using HarmonyLib;
using StardewModdingAPI;

namespace BetterDeconstructor;

// ReSharper disable once UnusedType.Global
internal class ModEntry : Mod
{
    // ReSharper disable once NotAccessedField.Global
    public static ModEntry? Instance;

    public override void Entry(IModHelper helper)
    {
        Instance = this;

        // 启用Harmony补丁
        Harmony harmony = new(ModManifest.UniqueID);
        harmony.PatchAll();
    }
}