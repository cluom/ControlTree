using ControlTree.Framework;
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.TerrainFeatures;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley.ItemTypeDefinitions;
using Object = StardewValley.Object;

namespace ControlTree.Patches
{
    [Harmony]
    public static class TreePatch
    {
        // ReSharper disable once InconsistentNaming
        // 模组配置
        private static ModConfig? Config;

        // ReSharper disable once InconsistentNaming
        // ReSharper disable once NotAccessedField.Local
        private static IMonitor? Monitor;

        // 被控制的树木集合
        private static readonly HashSet<NetString> ControlTreeType = new();

        // 贴图映射
        public static readonly Dictionary<string, Texture2D> TextureMapping = new();

        // 高亮框贴图
        private static readonly Texture2D TransparentTexture;

        static TreePatch()
        {
            // 初始化高亮框贴图
            TransparentTexture = CreateTransparentTexture(60, 60, 4);
        }

        public static void InitConfig(ModConfig config, IMonitor monitor)
        {
            Config = config;
            Monitor = monitor;
        }

        // 用于添加或移除被控制的树木
        public static void ChangeTreeType(NetString treeType, bool flag = true)
        {
            if (flag)
            {
                ControlTreeType.Add(treeType);
                return;
            }

            if (ControlTreeType.Contains(treeType))
            {
                ControlTreeType.Remove(treeType);
            }
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        // 用于绘制提示物品
        private static void DrawTipItem(Tree tree, ParsedItemData itemData, float offsetLayer = 0f)
        {
            var tileLocation = tree.Tile;
            var totalGameTime = Game1.currentGameTime.TotalGameTime;
            var globalPosition1 = tileLocation * 64f + new Vector2(
                -8.0f,
                (float)(4.0 * Math.Round(Math.Sin(totalGameTime.TotalMilliseconds / 250.0), 2)) - 64f
            );
            var vector22 = new Vector2(40f, 36f);
            Game1.spriteBatch.Draw(
                Game1.mouseCursors,
                Game1.GlobalToLocal(Game1.viewport, globalPosition1),
                new Rectangle(141, 465, 20, 24),
                Color.White * 0.75f,
                0.0f,
                Vector2.Zero,
                4f,
                SpriteEffects.None,
                1000f + tileLocation.Y + offsetLayer
            );

            Game1.spriteBatch.Draw(
                itemData.GetTexture(),
                Game1.GlobalToLocal(Game1.viewport, globalPosition1 + vector22),
                itemData.GetSourceRect(),
                Color.White * 0.75f,
                0.0f,
                new Vector2(8f, 8f),
                4f,
                SpriteEffects.None,
                1001f + tileLocation.Y + offsetLayer
            );
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        // 用于绘制高亮框
        private static void DrawHighlightBox(Tree tree)
        {
            if (Config is null) return;
            var tileLocation = tree.Tile;

            var sinValue = (float)Math.Abs(Math.Sin((double)DateTime.Now.Ticks / TimeSpan.TicksPerSecond * Math.PI));

            // 绘制高亮框
            Game1.spriteBatch.Draw(
                TransparentTexture,
                Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * 64f + 2f, tileLocation.Y * 64f + 2f)),
                null,
                // ReSharper disable once PossibleLossOfFraction
                Config.HighlightTreeSeedColor * (
                    0.2f + sinValue * 0.8f
                ),
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
            );
        }

        // 用于创建框状贴图
        private static Texture2D CreateTransparentTexture(int width, int height, int borderWidth)
        {
            var texture = new Texture2D(Game1.graphics.GraphicsDevice, width, height);
            var data = new Color[width * height];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var isBorder = y < borderWidth ||
                                   y >= height - borderWidth ||
                                   x < borderWidth ||
                                   x >= width - borderWidth;
                    data[y * width + x] = isBorder ? Color.White : Color.Transparent;
                }
            }

            texture.SetData(data);
            return texture;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Tree), "draw")]
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Global
        public static void PrefixDraw(Tree __instance)
        {
            if (Config is not { ModEnable: true })
            {
                return;
            }

            // 绘制种子高亮
            if (__instance.growthStage.Value == 0 && Config.HighlightTreeSeed)
            {
                var downTile = __instance.Tile + new Vector2(0, 1);
                Game1.currentLocation.terrainFeatures.TryGetValue(downTile, out var tempObject);
                if (Config.TransparentTree && tempObject is Tree tree && tree.growthStage.Value >= 5)
                {
                    tree.alpha = 0.2f;
                }
                DrawHighlightBox(__instance);
            }

            // 绘制种子提示
            if (__instance.hasSeed.Value && Config.ShowTreeSeedTips && __instance.growthStage.Value >= 5)
            {
                DrawTipItem(
                    __instance,
                    ItemRegistry.GetDataOrErrorItem(
                        ItemRegistry.Create(__instance.GetData().SeedItemId).QualifiedItemId
                    ),
                    5f
                );
            }

            // 绘制苔藓提示
            if (__instance.hasMoss.Value && Config.ShowTreeMossTips && __instance.growthStage.Value >= 5)
            {
                DrawTipItem(
                    __instance,
                    ItemRegistry.GetDataOrErrorItem(ItemRegistry.Create("(O)Moss").QualifiedItemId)
                );
            }

            var treeType = __instance.treeType;

            // 如果树木不在被控制的树木集合中 或者 树木是树桩且不是倒下的树木 或者 树木生长阶段小于5 则返回
            if (!ControlTreeType.Contains(treeType))
            {
                return;
            }

            if (__instance.stump.Value && !__instance.falling.Value)
            {
                return;
            }

            if (__instance.growthStage.Value < 5)
            {
                return;
            }

            // 设置标记CanChange为true
            SpriteBatchPatch.CanChange = true;
            if (__instance.TextureName is null)
            {
                return;
            }

            // 获取要替换的树木贴图 如果没有则返回 如果有则设置SpriteBatchPatch.Texture为对应的贴图
            var key = __instance.TextureName.Replace("TerrainFeatures\\", "") + ".png";
            if (TextureMapping.TryGetValue(key, out var value)) SpriteBatchPatch.Texture = value;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Tree), "draw")]
        // ReSharper disable once UnusedMember.Global
        public static void PostfixDraw()
        {
            // 重置各种标记
            SpriteBatchPatch.CanChange = false;
            SpriteBatchPatch.Texture = null;
        }
    }

    [Harmony]
    public static class SpriteBatchPatch
    {
        // ReSharper disable once InconsistentNaming
        private static ModConfig? Config;

        // 用于标记是否可以更改绘图逻辑
        public static bool CanChange { get; set; }

        // 用于设置要替换的贴图
        public static Texture2D? Texture { get; set; }

        public static void InitConfig(ModConfig config)
        {
            Config = config;
        }

        [
            HarmonyPrefix,
            HarmonyPatch(
                typeof(SpriteBatch),
                "Draw",
                new Type[]
                {
                    typeof(Texture2D),
                    typeof(Vector2),
                    typeof(Rectangle?),
                    typeof(Color),
                    typeof(float),
                    typeof(Vector2),
                    typeof(Vector2),
                    typeof(SpriteEffects),
                    typeof(float)
                }
            )
        ]
        // ReSharper disable once UnusedMember.Global
        public static bool Prefix_Draw(
            // ReSharper disable once InconsistentNaming
            SpriteBatch __instance,
            [HarmonyArgument("texture")] ref Texture2D texture,
            [HarmonyArgument("position")] ref Vector2 position,
            Rectangle? sourceRectangle,
            Color color, float rotation,
            Vector2 origin,
            [HarmonyArgument("scale")] ref Vector2 scale,
            SpriteEffects effects, float layerDepth
        )
        {
            if (!CanChange) return true;
            switch (Config)
            {
                // 如果不渲染树干且当前渲染的是树干 则取消渲染
                case { RenderTreeTrunk: false } when origin != Vector2.Zero:
                    return false;
                // 如果不渲染树干则直接返回
                case { RenderTreeTrunk: false }:
                    // 如果不渲染树叶影子且当前渲染的是树叶影子 则取消渲染
                    return Config.RenderLeafyShadow || texture != Game1.mouseCursors;
                // 如果不换贴图也不缩小则直接返回
                case { TextureChange: false, MinishTree: false }:
                    return true;
            }

            // 如果 要缩小树 或 (要替换贴图 并且 当前渲染的是影子)
            if (
                Config is { MinishTree: true } || (
                    Config is { TextureChange: true } && (
                        texture == Game1.mouseCursors ||
                        texture == Game1.mouseCursors_1_6
                    )
                )
            )
            {
                // 缩小比例
                scale *= 0.5f;

                if (sourceRectangle is not null)
                {
                    // 根据源矩形信息调整位置
                    var rect = sourceRectangle.Value;
                    position.X += rect.Width;
                    position.Y += rect.Height;
                    if (texture != Game1.mouseCursors && texture != Game1.mouseCursors_1_6)
                    {
                        position.Y += 16f;
                    }
                }

                // 根据原点调整位置
                position -= origin * 2f;
                position.Y += origin.Y * 0.75f;
            }

            // 如果要替换贴图 并且 当前渲染的不是影子 则替换贴图
            if (
                Config is { TextureChange: true } &&
                Texture is not null &&
                texture != Game1.mouseCursors &&
                texture != Game1.mouseCursors_1_6
            )
            {
                texture = Texture;
            }

            return true;
        }
    }
}