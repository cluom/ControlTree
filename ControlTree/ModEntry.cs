using GenericModConfigMenu;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace ControlTree
{
    // ReSharper disable once UnusedType.Global
    internal class ModEntry : Mod
    {
        // ReSharper disable once InconsistentNaming
        private ModConfig Config = null!;

        public override void Entry(IModHelper helper)
        {
            // Monitor.Log("Hello World", LogLevel.Debug);
            Config = Helper.ReadConfig<ModConfig>();

            helper.Events.Input.ButtonsChanged += OnButtonsChanged!;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched!;

            if (Config.MinishOak) TreePatch.ChangeMinishTreeType(TreeTypeEnum.Oak.Id);
            if (Config.MinishMaple) TreePatch.ChangeMinishTreeType(TreeTypeEnum.Maple.Id);
            if (Config.MinishPine) TreePatch.ChangeMinishTreeType(TreeTypeEnum.Pine.Id);
            if (Config.MinishMahogany) TreePatch.ChangeMinishTreeType(TreeTypeEnum.Mahogany.Id);
            if (Config.MinishGreenRainType1) TreePatch.ChangeMinishTreeType(TreeTypeEnum.GreenRainType1.Id);
            if (Config.MinishGreenRainType2) TreePatch.ChangeMinishTreeType(TreeTypeEnum.GreenRainType2.Id);
            if (Config.MinishGreenRainType3) TreePatch.ChangeMinishTreeType(TreeTypeEnum.GreenRainType3.Id);
            if (Config.MinishMystic) TreePatch.ChangeMinishTreeType(TreeTypeEnum.Mystic.Id);

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
                name: () => Helper.Translation.Get("config.highlight_tree_seed.name"),
                tooltip: () => Helper.Translation.Get("config.highlight_tree_seed.tooltip"),
                getValue: () => Config.HighlightTreeSeed,
                setValue: value => Config.HighlightTreeSeed = value
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
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.minish_oak.name"),
                tooltip: () => Helper.Translation.Get("config.minish_oak.tooltip"),
                getValue: () => Config.MinishOak,
                setValue: value =>
                {
                    Config.MinishOak = value;
                    TreePatch.ChangeMinishTreeType(TreeTypeEnum.Oak.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.minish_maple.name"),
                tooltip: () => Helper.Translation.Get("config.minish_maple.tooltip"),
                getValue: () => Config.MinishMaple,
                setValue: value =>
                {
                    Config.MinishMaple = value;
                    TreePatch.ChangeMinishTreeType(TreeTypeEnum.Maple.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.minish_pine.name"),
                tooltip: () => Helper.Translation.Get("config.minish_pine.tooltip"),
                getValue: () => Config.MinishPine,
                setValue: value =>
                {
                    Config.MinishPine = value;
                    TreePatch.ChangeMinishTreeType(TreeTypeEnum.Pine.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.minish_mahogany.name"),
                tooltip: () => Helper.Translation.Get("config.minish_mahogany.tooltip"),
                getValue: () => Config.MinishMahogany,
                setValue: value =>
                {
                    Config.MinishMahogany = value;
                    TreePatch.ChangeMinishTreeType(TreeTypeEnum.Mahogany.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.minish_green_rain_type1.name"),
                tooltip: () => Helper.Translation.Get("config.minish_green_rain_type1.tooltip"),
                getValue: () => Config.MinishGreenRainType1,
                setValue: value =>
                {
                    Config.MinishGreenRainType1 = value;
                    TreePatch.ChangeMinishTreeType(TreeTypeEnum.GreenRainType1.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.minish_green_rain_type2.name"),
                tooltip: () => Helper.Translation.Get("config.minish_green_rain_type2.tooltip"),
                getValue: () => Config.MinishGreenRainType2,
                setValue: value =>
                {
                    Config.MinishGreenRainType2 = value;
                    TreePatch.ChangeMinishTreeType(TreeTypeEnum.GreenRainType2.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.minish_green_rain_type3.name"),
                tooltip: () => Helper.Translation.Get("config.minish_green_rain_type3.tooltip"),
                getValue: () => Config.MinishGreenRainType3,
                setValue: value =>
                {
                    Config.MinishGreenRainType3 = value;
                    TreePatch.ChangeMinishTreeType(TreeTypeEnum.GreenRainType3.Id, value);
                }
            );
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => Helper.Translation.Get("config.minish_mystic.name"),
                tooltip: () => Helper.Translation.Get("config.minish_mystic.tooltip"),
                getValue: () => Config.MinishMystic,
                setValue: value =>
                {
                    Config.MinishMystic = value;
                    TreePatch.ChangeMinishTreeType(TreeTypeEnum.Mystic.Id, value);
                }
            );
        }

        private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (!ShouldEnable(forInput: true)) return;
            if (!Config.ModEnableToggleKey.JustPressed()) return;
            Config.ModEnable = !Config.ModEnable;
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