using Microsoft.Xna.Framework;
using StardewModdingAPI.Utilities;

namespace ControlTree
{
    public sealed class ModConfig
    {
        public bool ModEnable { get; set; } = true;
        public KeybindList ModEnableToggleKey { get; set; } = KeybindList.Parse("L");
        public bool TextureChange { get; set; } = true;
        public KeybindList TextureChangeToggleKey { get; set; } = KeybindList.Parse("");
        public bool MinishTree { get; set; }
        public KeybindList MinishTreeToggleKey { get; set; } = KeybindList.Parse("");
        public bool HighlightTreeSeed { get; set; }
        public KeybindList HighlightTreeSeedToggleKey { get; set; } = KeybindList.Parse("");
        public bool ShowTreeSeedTips { get; set; } = true;
        public KeybindList ShowTreeSeedTipsToggleKey { get; set; } = KeybindList.Parse("");
        public bool ShowTreeMossTips { get; set; } = true;
        public KeybindList ShowTreeMossTipsToggleKey { get; set; } = KeybindList.Parse("");
        
        public Color HighlightTreeSeedColor { get; set; } = Color.Red;
        public bool RenderTreeTrunk { get; set; } = true;
        public bool RenderLeafyShadow { get; set; } = true;
        public bool ChangeOak { get; set; } = true;
        public bool ChangeMaple { get; set; } = true;
        public bool ChangePine { get; set; } = true;
        public bool ChangeMahogany { get; set; } = true;
        public bool ChangeGreenRainType1 { get; set; } = true;
        public bool ChangeGreenRainType2 { get; set; } = true;
        public bool ChangeGreenRainType3 { get; set; } = true;
        public bool ChangeMystic { get; set; } = true;
    }
}
