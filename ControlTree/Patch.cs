
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.TerrainFeatures;
using StardewValley;
using Microsoft.Xna.Framework;

namespace ControlTree
{
    [Harmony]
    public static class TreePatch
    {
        // ReSharper disable once InconsistentNaming
        private static ModConfig? Config;
        private static readonly HashSet<NetString> MinishTreeType = new();

        public static void InitConfig(ModConfig config)
        {
            Config = config;
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


        [HarmonyPrefix, HarmonyPatch(typeof(Tree), "draw")]
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Global
        public static void PrefixDraw(Tree __instance)
        {
            if (Config is not { ModEnable: true }) { return; }

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
