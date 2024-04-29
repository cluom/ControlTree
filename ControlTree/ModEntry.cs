using GenericModConfigMenu;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace ControlTree
{
    // ReSharper disable once UnusedType.Global
    internal class ModEntry : Mod
    {
        // ReSharper disable once InconsistentNaming
        private ModConfig Config = null!;
        private readonly Texture2D _highlightTreeSeedColorTexture = new(Game1.graphics.GraphicsDevice, 1, 1);
        private Color _highlightTreeSeedColor = Color.Red;

        public override void Entry(IModHelper helper)
        {
            // Monitor.Log("Hello World", LogLevel.Debug);
            Config = Helper.ReadConfig<ModConfig>();

            helper.Events.Input.ButtonsChanged += OnButtonsChanged!;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched!;

            if (Config.ChangeOak) TreePatch.ChangeTreeType(TreeTypeEnum.Oak.Id);
            if (Config.ChangeMaple) TreePatch.ChangeTreeType(TreeTypeEnum.Maple.Id);
            if (Config.ChangePine) TreePatch.ChangeTreeType(TreeTypeEnum.Pine.Id);
            if (Config.ChangePine) TreePatch.ChangeTreeType(TreeTypeEnum.Mushroom.Id);
            if (Config.ChangeMahogany) TreePatch.ChangeTreeType(TreeTypeEnum.Mahogany.Id);
            if (Config.ChangeGreenRainType1) TreePatch.ChangeTreeType(TreeTypeEnum.GreenRainType1.Id);
            if (Config.ChangeGreenRainType2) TreePatch.ChangeTreeType(TreeTypeEnum.GreenRainType2.Id);
            if (Config.ChangeGreenRainType3) TreePatch.ChangeTreeType(TreeTypeEnum.GreenRainType3.Id);
            if (Config.ChangeMystic) TreePatch.ChangeTreeType(TreeTypeEnum.Mystic.Id);

            _highlightTreeSeedColorTexture.SetData(new[] { Config.HighlightTreeSeedColor });
            _highlightTreeSeedColor = Config.HighlightTreeSeedColor;

            foreach (var textureName in helper.ModContent.Load<List<string>>("assets/textures.json"))
            {
                Monitor.Log($"Load texture: {textureName}");
                TreePatch.TextureMapping[textureName] = helper.ModContent.Load<Texture2D>($"assets/{textureName}");
            }

            TreePatch.InitConfig(Config, Monitor);
            SpriteBatchPatch.InitConfig(Config);

            Harmony harmony = new(ModManifest.UniqueID);
            harmony.PatchAll();
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            configMenu.Register(
                mod: ModManifest,
                reset: () => Config = new ModConfig(),
                save: () => Helper.WriteConfig(Config)
            );

            configMenu.AddSectionTitle(
                mod: ModManifest,
                text: () => Helper.Translation.Get("config.title.text")
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.mod_enable.name"),
                tooltip: () => Helper.Translation.Get("config.mod_enable.tooltip"),
                getValue: () => Config.ModEnable,
                setValue: value => Config.ModEnable = value
            );
            configMenu.AddKeybindList(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.mod_enable_toggle_key.name"),
                tooltip: () => Helper.Translation.Get("config.mod_enable_toggle_key.tooltip"),
                getValue: () => Config.ModEnableToggleKey,
                setValue: value => Config.ModEnableToggleKey = value
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.texture_change.name"),
                tooltip: () => Helper.Translation.Get("config.texture_change.tooltip"),
                getValue: () => Config.TextureChange,
                setValue: value => Config.TextureChange = value
            );
            configMenu.AddKeybindList(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.texture_change_toggle_key.name"),
                tooltip: () => Helper.Translation.Get("config.texture_change_toggle_key.tooltip"),
                getValue: () => Config.TextureChangeToggleKey,
                setValue: value => Config.TextureChangeToggleKey = value
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.minish_tree.name"),
                tooltip: () => Helper.Translation.Get("config.minish_tree.tooltip"),
                getValue: () => Config.MinishTree,
                setValue: value => Config.MinishTree = value
            );
            configMenu.AddKeybindList(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.minish_tree_toggle_key.name"),
                tooltip: () => Helper.Translation.Get("config.minish_tree_toggle_key.tooltip"),
                getValue: () => Config.MinishTreeToggleKey,
                setValue: value => Config.MinishTreeToggleKey = value
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.highlight_tree_seed.name"),
                tooltip: () => Helper.Translation.Get("config.highlight_tree_seed.tooltip"),
                getValue: () => Config.HighlightTreeSeed,
                setValue: value => Config.HighlightTreeSeed = value
            );
            configMenu.AddKeybindList(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.highlight_tree_seed_toggle_key.name"),
                tooltip: () => Helper.Translation.Get("config.highlight_tree_seed_toggle_key.tooltip"),
                getValue: () => Config.HighlightTreeSeedToggleKey,
                setValue: value => Config.HighlightTreeSeedToggleKey = value
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.show_tree_seed_tips.name"),
                tooltip: () => Helper.Translation.Get("config.show_tree_seed_tips.tooltip"),
                getValue: () => Config.ShowTreeSeedTips,
                setValue: value => Config.ShowTreeSeedTips = value
            );
            configMenu.AddKeybindList(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.show_tree_seed_tips_toggle_key.name"),
                tooltip: () => Helper.Translation.Get("config.show_tree_seed_tips_toggle_key.tooltip"),
                getValue: () => Config.ShowTreeSeedTipsToggleKey,
                setValue: value => Config.ShowTreeSeedTipsToggleKey = value
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.show_tree_moss_tips.name"),
                tooltip: () => Helper.Translation.Get("config.show_tree_moss_tips.tooltip"),
                getValue: () => Config.ShowTreeMossTips,
                setValue: value => Config.ShowTreeMossTips = value
            );
            configMenu.AddKeybindList(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.show_tree_moss_tips_toggle_key.name"),
                tooltip: () => Helper.Translation.Get("config.show_tree_moss_tips_toggle_key.tooltip"),
                getValue: () => Config.ShowTreeMossTipsToggleKey,
                setValue: value => Config.ShowTreeMossTipsToggleKey = value
            );
            configMenu.AddImage(
                mod: ModManifest,
                texture: () => _highlightTreeSeedColorTexture
            );
            configMenu.AddNumberOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.highlight_tree_seed_color.name"),
                tooltip: () => Helper.Translation.Get("config.highlight_tree_seed_color.tooltip"),
                min: 0,
                max: 255,
                getValue: () => Config.HighlightTreeSeedColor.R,
                setValue: value =>
                {
                    var newColor = Config.HighlightTreeSeedColor;
                    newColor.R = (byte)value;
                    _highlightTreeSeedColorTexture.SetData(new[] { newColor });
                    Config.HighlightTreeSeedColor = newColor;
                },
                formatValue: i =>
                {
                    if (_highlightTreeSeedColor.R == i) return $"R: {i:X}";
                    ;
                    _highlightTreeSeedColor.R = (byte)i;
                    _highlightTreeSeedColorTexture.SetData(new[] { _highlightTreeSeedColor });
                    return $"R: {i:X}";
                }
            );
            configMenu.AddNumberOption(
                mod: ModManifest,
                name: () => "",
                min: 0,
                max: 255,
                getValue: () => Config.HighlightTreeSeedColor.G,
                setValue: value =>
                {
                    var newColor = Config.HighlightTreeSeedColor;
                    newColor.G = (byte)value;
                    _highlightTreeSeedColorTexture.SetData(new[] { newColor });
                    Config.HighlightTreeSeedColor = newColor;
                },
                formatValue: i =>
                {
                    if (_highlightTreeSeedColor.G == i) return $"G: {i:X}";
                    ;
                    _highlightTreeSeedColor.G = (byte)i;
                    _highlightTreeSeedColorTexture.SetData(new[] { _highlightTreeSeedColor });
                    return $"G: {i:X}";
                }
            );
            configMenu.AddNumberOption(
                mod: ModManifest,
                name: () => "",
                min: 0,
                max: 255,
                getValue: () => Config.HighlightTreeSeedColor.B,
                setValue: value =>
                {
                    var newColor = Config.HighlightTreeSeedColor;
                    newColor.B = (byte)value;
                    _highlightTreeSeedColorTexture.SetData(new[] { newColor });
                    Config.HighlightTreeSeedColor = newColor;
                },
                formatValue: i =>
                {
                    if (_highlightTreeSeedColor.B == i) return $"B: {i:X}";
                    ;
                    _highlightTreeSeedColor.B = (byte)i;
                    _highlightTreeSeedColorTexture.SetData(new[] { _highlightTreeSeedColor });
                    return $"B: {i:X}";
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.render_tree_trunk.name"),
                tooltip: () => Helper.Translation.Get("config.render_tree_trunk.tooltip"),
                getValue: () => Config.RenderTreeTrunk,
                setValue: value => Config.RenderTreeTrunk = value
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.render_leafy_shadow.name"),
                tooltip: () => Helper.Translation.Get("config.render_leafy_shadow.tooltip"),
                getValue: () => Config.RenderLeafyShadow,
                setValue: value => Config.RenderLeafyShadow = value
            );
            configMenu.AddParagraph(
                mod: ModManifest,
                text: () => ""
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.change_oak.name"),
                tooltip: () => Helper.Translation.Get("config.change_oak.tooltip"),
                getValue: () => Config.ChangeOak,
                setValue: value =>
                {
                    Config.ChangeOak = value;
                    TreePatch.ChangeTreeType(TreeTypeEnum.Oak.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.change_maple.name"),
                tooltip: () => Helper.Translation.Get("config.change_maple.tooltip"),
                getValue: () => Config.ChangeMaple,
                setValue: value =>
                {
                    Config.ChangeMaple = value;
                    TreePatch.ChangeTreeType(TreeTypeEnum.Maple.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.change_pine.name"),
                tooltip: () => Helper.Translation.Get("config.change_pine.tooltip"),
                getValue: () => Config.ChangePine,
                setValue: value =>
                {
                    Config.ChangePine = value;
                    TreePatch.ChangeTreeType(TreeTypeEnum.Pine.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.change_mushroom.name"),
                tooltip: () => Helper.Translation.Get("config.change_mushroom.tooltip"),
                getValue: () => Config.ChangeMushroom,
                setValue: value =>
                {
                    Config.ChangeMushroom = value;
                    TreePatch.ChangeTreeType(TreeTypeEnum.Mushroom.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.change_mahogany.name"),
                tooltip: () => Helper.Translation.Get("config.change_mahogany.tooltip"),
                getValue: () => Config.ChangeMahogany,
                setValue: value =>
                {
                    Config.ChangeMahogany = value;
                    TreePatch.ChangeTreeType(TreeTypeEnum.Mahogany.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.change_green_rain_type1.name"),
                tooltip: () => Helper.Translation.Get("config.change_green_rain_type1.tooltip"),
                getValue: () => Config.ChangeGreenRainType1,
                setValue: value =>
                {
                    Config.ChangeGreenRainType1 = value;
                    TreePatch.ChangeTreeType(TreeTypeEnum.GreenRainType1.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.change_green_rain_type2.name"),
                tooltip: () => Helper.Translation.Get("config.change_green_rain_type2.tooltip"),
                getValue: () => Config.ChangeGreenRainType2,
                setValue: value =>
                {
                    Config.ChangeGreenRainType2 = value;
                    TreePatch.ChangeTreeType(TreeTypeEnum.GreenRainType2.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.change_green_rain_type3.name"),
                tooltip: () => Helper.Translation.Get("config.change_green_rain_type3.tooltip"),
                getValue: () => Config.ChangeGreenRainType3,
                setValue: value =>
                {
                    Config.ChangeGreenRainType3 = value;
                    TreePatch.ChangeTreeType(TreeTypeEnum.GreenRainType3.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.change_mystic.name"),
                tooltip: () => Helper.Translation.Get("config.change_mystic.tooltip"),
                getValue: () => Config.ChangeMystic,
                setValue: value =>
                {
                    Config.ChangeMystic = value;
                    TreePatch.ChangeTreeType(TreeTypeEnum.Mystic.Id, value);
                }
            );
        }

        private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (!ShouldEnable(forInput: true)) return;
            
            ToggleConfigOption(Config.ModEnableToggleKey, () => Config.ModEnable = !Config.ModEnable);
            ToggleConfigOption(Config.TextureChangeToggleKey, () => Config.TextureChange = !Config.TextureChange);
            ToggleConfigOption(Config.MinishTreeToggleKey, () => Config.MinishTree = !Config.MinishTree);
            ToggleConfigOption(Config.HighlightTreeSeedToggleKey, () => Config.HighlightTreeSeed = !Config.HighlightTreeSeed);
            ToggleConfigOption(Config.ShowTreeSeedTipsToggleKey, () => Config.ShowTreeSeedTips = !Config.ShowTreeSeedTips);
            ToggleConfigOption(Config.ShowTreeMossTipsToggleKey, () => Config.ShowTreeMossTips = !Config.ShowTreeMossTips);
        }
        
        private void ToggleConfigOption(KeybindList keyBind, Action toggleAction)
        {
            if (!keyBind.JustPressed()) return;
            toggleAction();
            Helper.WriteConfig(Config);
        }

        private static bool ShouldEnable(bool forInput = false)
        {
            if (!Context.IsWorldReady || !Context.IsMainPlayer) return false;

            if (!forInput) return true;

            if (!Context.IsPlayerFree && !Game1.eventUp) return false;

            return Game1.keyboardDispatcher.Subscriber == null;
        }
    }
}