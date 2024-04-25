using StardewModdingAPI.Utilities;

namespace ControlTree
{
    public sealed class ModConfig
    {
        public bool ModEnable { get; set; } = true;
        public KeybindList ModEnableToggleKey { get; set; } = KeybindList.Parse("L");
        public bool RenderTreeTrunk { get; set; } = true;
        public bool RenderLeafyShadow { get; set; } = true;
        public bool MinishOak { get; set; } = true;
        public bool MinishMaple { get; set; } = true;
        public bool MinishPine { get; set; } = true;
        public bool MinishMahogany { get; set; } = true;
        public bool MinishGreenRainType1 { get; set; } = true;
        public bool MinishGreenRainType2 { get; set; } = true;
        public bool MinishGreenRainType3 { get; set; } = true;
        public bool MinishMystic { get; set; } = true;
    }
}
