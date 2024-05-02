using HarmonyLib;
using StardewValley;
using StardewValley.Extensions;
using StardewValley.GameData.Machines;
using SObject = StardewValley.Object;

namespace BetterDeconstructor.Patches;

[Harmony]
// ReSharper disable once UnusedType.Global
public class ObjectPatch
{
    [HarmonyPostfix, HarmonyPatch(typeof(SObject), "OutputDeconstructor")]
    public static void Postfix_OutputDeconstructor(
        SObject __instance,
        SObject machine,
        Item inputItem,
        bool probe,
        MachineItemOutput outputData,
        ref Item? __result)
    {
        if (!inputItem.HasTypeObject() && !inputItem.HasTypeBigCraftable())
        {
            return;
        }

        if (!CraftingRecipe.craftingRecipes.TryGetValue(inputItem.Name, out var str))
        {
            return;
        }

        var array1 = str.Split('/');
        if (ArgUtility.SplitBySpace(ArgUtility.Get(array1, 2)).Length <= 1) return;

        var inputCount = int.Parse(ArgUtility.SplitBySpace(ArgUtility.Get(array1, 2))[1]);
        if (inputItem.Stack < inputCount) return;

        SObject? object1 = null;
        string[] array2 = ArgUtility.SplitBySpace(ArgUtility.Get(array1, 0));
        for (var index = 0; index < array2.Length; index += 2)
        {
            var object2 = new SObject(ArgUtility.Get(array2, index), ArgUtility.GetInt(array2, index + 1, 1));
            if (
                object1 != null &&
                object2.sellToStorePrice(-1L) * object2.Stack <= object1.sellToStorePrice() * object1.Stack
            ) continue;
            object1 = object2;
        }

        __result = object1;
        if (object1 is null) return;
        inputItem.Stack -= inputCount + 1;
    }
}