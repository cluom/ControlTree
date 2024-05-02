using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using SObject = StardewValley.Object;

namespace ControlTree.Patches;

[Harmony]
// ReSharper disable once UnusedType.Global
public class TapperPatch
{
    private static SObject? _heldObjectValue;

    [
        HarmonyPrefix,
        HarmonyPatch(
            typeof(SObject),
            "draw",
            typeof(SpriteBatch), typeof(int), typeof(int), typeof(float)
        )
    ]
    // ReSharper disable once InconsistentNaming
    public static bool Prefix_Tapper_Draw(SObject __instance)
    {
        if (ModEntry.Instance is null) return true;
        if (!ModEntry.Instance.Config.HideTapperProduct) return true;
        if (!__instance.IsTapper()) return true;
        if (!__instance.readyForHarvest.Value) return true;

        _heldObjectValue = __instance.heldObject.Value;
        __instance.heldObject.Value = null;
        __instance.readyForHarvest.Value = false;

        return true;
    }

    [
        HarmonyPostfix,
        HarmonyPatch(
            typeof(SObject),
            "draw",
            typeof(SpriteBatch), typeof(int), typeof(int), typeof(float)
        )
    ]
    // ReSharper disable once InconsistentNaming
    public static void Postfix_Tapper_Draw(SObject __instance)
    {
        if (!__instance.IsTapper()) return;
        if (_heldObjectValue is null) return;

        __instance.heldObject.Value = _heldObjectValue;
        __instance.readyForHarvest.Value = true;
        _heldObjectValue = null;
    }
}