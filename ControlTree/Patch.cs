
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.TerrainFeatures;
using StardewValley;
using Microsoft.Xna.Framework;
using System.Linq;

namespace ControlTree
{
    [Harmony]
    public static class TreePatch
    {
        private static ModConfig? Config;
        private static readonly HashSet<NetString> minishTreeType = new();

        public static void InitConfig(ModConfig config)
        {
            Config = config;
        }

        public static void ChangeMinishTreeType(NetString treeType, bool flag = true) {
            if (flag)
            {
                minishTreeType.Add(treeType);
                return;
            }
            if (minishTreeType.Contains(treeType)) {
                minishTreeType.Remove(treeType);
            }
        }


        [HarmonyPrefix, HarmonyPatch(typeof(Tree), "draw")]
        public static void PrefixDraw(Tree __instance)
        {
            if (Config == null || !Config.ModEnable) { return; }

            var treeType = __instance.treeType;

            if (!minishTreeType.Contains(treeType)) { return; }
            if (__instance.stump.Value && !(bool)__instance.falling.Value) { return; }
            if ((int)__instance.growthStage.Value < 5) { return; }

            SpriteBatchPatch.canMinish = true;

        }

        [HarmonyPostfix, HarmonyPatch(typeof(Tree), "draw")]
        public static void PostfixDraw()
        {
            SpriteBatchPatch.canMinish = false;
        }
    }

    [Harmony]
    public static class SpriteBatchPatch
    {
        private static ModConfig? Config;
        public static bool canMinish { get; set; } = false;

        public static void InitConfig(ModConfig config)
        {
            Config = config;
        }


        [HarmonyPrefix, HarmonyPatch(typeof(SpriteBatch), "Draw", new Type[] { typeof(Texture2D), typeof(Vector2), typeof(Rectangle?), typeof(Color), typeof(float), typeof(Vector2), typeof(Vector2), typeof(SpriteEffects), typeof(float) })]
        public static bool Prefix_Draw(SpriteBatch __instance, Texture2D texture, [HarmonyArgument("position")] ref Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, [HarmonyArgument("scale")] ref Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (canMinish)
            {
                if (Config!= null && !Config.RenderTreeTrunk)
                {
                    if (origin != Vector2.Zero) { return false; }
                    if (!Config.RenderLeafyShadow && texture == Game1.mouseCursors) { return false; }
                    return true;
                }

                // 缩小比例
                scale *= 0.5f;

                if (sourceRectangle != null)
                {
                    Rectangle rect = sourceRectangle.Value;
                    position.X += rect.Width;
                    position.Y += rect.Height;
                    if (texture != Game1.mouseCursors && texture != Game1.mouseCursors_1_6) { position.Y += 16f; }
                }

                position -= origin * 2f;
                position.Y += origin.Y * 0.75f;
            }

            return true;
        }
    }
}
