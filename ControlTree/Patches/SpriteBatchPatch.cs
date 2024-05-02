using ControlTree.Framework;
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using Microsoft.Xna.Framework;

namespace ControlTree.Patches;

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