using Microsoft.Xna.Framework;
using StardewModdingAPI.Utilities;

namespace ControlTree
{
    public sealed class ModConfig
    {
        // 模组开关
        public bool ModEnable { get; set; } = true;

        // 模组开关快捷键
        public KeybindList ModEnableToggleKey { get; set; } = KeybindList.Parse("L");

        // 树木贴图更换开关
        public bool TextureChange { get; set; } = true;

        // 树木贴图更换开关快捷键
        public KeybindList TextureChangeToggleKey { get; set; } = KeybindList.Parse("");

        // 树木缩小开关
        public bool MinishTree { get; set; }

        // 树木缩小开关快捷键
        public KeybindList MinishTreeToggleKey { get; set; } = KeybindList.Parse("");

        // 树木种子高亮开关
        public bool HighlightTreeSeed { get; set; }

        // 树木种子高亮开关快捷键
        public KeybindList HighlightTreeSeedToggleKey { get; set; } = KeybindList.Parse("");

        // 树木种子提示开关
        public bool ShowTreeSeedTips { get; set; } = true;

        // 树木种子提示开关快捷键
        public KeybindList ShowTreeSeedTipsToggleKey { get; set; } = KeybindList.Parse("");

        // 树木苔藓提示开关
        public bool ShowTreeMossTips { get; set; } = true;

        // 树木苔藓提示开关快捷键
        public KeybindList ShowTreeMossTipsToggleKey { get; set; } = KeybindList.Parse("");

        // 树木种子高亮颜色
        public Color HighlightTreeSeedColor { get; set; } = Color.Red;

        // 渲染树干开关
        public bool RenderTreeTrunk { get; set; } = true;

        // 渲染树干开关快捷键
        public KeybindList RenderTreeTrunkToggleKey { get; set; } = KeybindList.Parse("");

        // 渲染树叶影子开关
        public bool RenderLeafyShadow { get; set; } = true;

        // 渲染树叶影子开关快捷键
        public KeybindList RenderLeafyShadowToggleKey { get; set; } = KeybindList.Parse("");

        // 橡树
        public bool ChangeOak { get; set; } = true;

        // 枫树
        public bool ChangeMaple { get; set; } = true;

        // 松树
        public bool ChangePine { get; set; } = true;

        // 蘑菇树
        public bool ChangeMushroom { get; set; } = true;

        // 桃花心木
        public bool ChangeMahogany { get; set; } = true;

        // 苔藓树1
        public bool ChangeGreenRainType1 { get; set; } = true;

        // 苔藓树2
        public bool ChangeGreenRainType2 { get; set; } = true;

        // 蕨树
        public bool ChangeGreenRainType3 { get; set; } = true;

        // 神秘树
        public bool ChangeMystic { get; set; } = true;
    }
}