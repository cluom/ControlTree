
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.TerrainFeatures;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace ControlTree
{
    [Harmony]
    public static class TreePatch
    {
        // ReSharper disable once InconsistentNaming
        private static ModConfig? Config;
        // ReSharper disable once InconsistentNaming
        private static IMonitor? Monitor;
        private static readonly HashSet<NetString> MinishTreeType = new();


        public static void InitConfig(ModConfig config, IMonitor monitor)
        {
            Config = config;
            Monitor = monitor;
        }

        public static void ChangeMinishTreeType(NetString treeType, bool flag = true) {
            if (flag)
            {
                MinishTreeType.Add(treeType);
                return;
            }
            if (MinishTreeType.Contains(treeType)) {
                MinishTreeType.Remove(treeType);
            }
        }
        
        public static Texture2D CreateTransparentTexture(int width, int height, int borderWidth, Color borderColor)
        {
            var texture = new Texture2D(Game1.graphics.GraphicsDevice, width, height);
            var data = new Color[width * height];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var isBorder = y < borderWidth || y >= height - borderWidth || x < borderWidth || x >= width - borderWidth;
                    data[y * width + x] = isBorder ? borderColor : Color.Transparent;
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
            if (Config is not { ModEnable: true }) { return; }
            

            if (__instance.growthStage.Value == 0 && Config.HighlightTreeSeed)
            {
                var tileLocation = __instance.Tile;

                // 绘制红色的框
                Game1.spriteBatch.Draw(
                    CreateTransparentTexture(60, 60, 4, Color.Red),
                    Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * 64f + 2f, tileLocation.Y * 64f + 2f)),
                    null, 
                    // ReSharper disable once PossibleLossOfFraction
                    Color.Red * (0.2f + (float)Math.Abs(Math.Sin((double)DateTime.Now.Ticks / TimeSpan.TicksPerSecond * Math.PI)) * 0.8f),
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }

            var treeType = __instance.treeType;

            if (!MinishTreeType.Contains(treeType)) { return; }
            if (__instance.stump.Value && !__instance.falling.Value) { return; }
            if (__instance.growthStage.Value < 5) { return; }

            SpriteBatchPatch.CanMinish = true;

        }

        [HarmonyPostfix, HarmonyPatch(typeof(Tree), "draw")]
        // ReSharper disable once UnusedMember.Global
        public static void PostfixDraw()
        {
            SpriteBatchPatch.CanMinish = false;
        }
    }

    [Harmony]
    public static class SpriteBatchPatch
    {
        // ReSharper disable once InconsistentNaming
        private static ModConfig? Config;
        public static bool CanMinish { get; set; }

        public static void InitConfig(ModConfig config)
        {
            Config = config;
        }


        [HarmonyPrefix, HarmonyPatch(typeof(SpriteBatch), "Draw", new Type[] { typeof(Texture2D), typeof(Vector2), typeof(Rectangle?), typeof(Color), typeof(float), typeof(Vector2), typeof(Vector2), typeof(SpriteEffects), typeof(float) })]
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedParameter.Global
        // ReSharper disable once UnusedMember.Global
        public static bool Prefix_Draw(SpriteBatch __instance, Texture2D texture, [HarmonyArgument("position")] ref Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, [HarmonyArgument("scale")] ref Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (!CanMinish) return true;
            if (Config is { RenderTreeTrunk: false })
            {
                if (origin != Vector2.Zero) { return false; }
                return Config.RenderLeafyShadow || texture != Game1.mouseCursors;
            }

            // 缩小比例
            scale *= 0.5f;

            if (sourceRectangle != null)
            {
                var rect = sourceRectangle.Value;
                position.X += rect.Width;
                position.Y += rect.Height;
                if (texture != Game1.mouseCursors && texture != Game1.mouseCursors_1_6) { position.Y += 16f; }
            }

            position -= origin * 2f;
            position.Y += origin.Y * 0.75f;

            return true;
        }
    }
}
