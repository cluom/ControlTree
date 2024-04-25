using GenericModConfigMenu;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace ControlTree
{
    internal class ModEntry : Mod
    {
        private ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            // this.Monitor.Log("Hello World", LogLevel.Debug);
            Config = this.Helper.ReadConfig<ModConfig>();

            helper.Events.Input.ButtonsChanged += OnButtonsChanged;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            if (Config != null)
            {
                if (Config.MinishOak) { TreePatch.ChangeMinishTreeType(TreeTypeEnum.Oak.Id, true); }
                if (Config.MinishMaple) { TreePatch.ChangeMinishTreeType(TreeTypeEnum.Maple.Id, true); }
                if (Config.MinishPine) { TreePatch.ChangeMinishTreeType(TreeTypeEnum.Pine.Id, true); }
                if (Config.MinishMahogany) { TreePatch.ChangeMinishTreeType(TreeTypeEnum.Mahogany.Id, true); }
                if (Config.MinishGreenRainType1) { TreePatch.ChangeMinishTreeType(TreeTypeEnum.GreenRainType1.Id, true); }
                if (Config.MinishGreenRainType2) { TreePatch.ChangeMinishTreeType(TreeTypeEnum.GreenRainType2.Id, true); }
                if (Config.MinishGreenRainType3) { TreePatch.ChangeMinishTreeType(TreeTypeEnum.GreenRainType3.Id, true); }
                if (Config.MinishMystic) { TreePatch.ChangeMinishTreeType(TreeTypeEnum.Mystic.Id, true); }
            }

            TreePatch.InitConfig(Config);
            SpriteBatchPatch.InitConfig(Config);

            Harmony harmony = new(ModManifest.UniqueID);
            harmony.PatchAll();
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config)
            );

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.mod_enable.name"),
                tooltip: () => this.Helper.Translation.Get("config.mod_enable.tooltip"),
                getValue: () => this.Config.ModEnable,
                setValue: value => this.Config.ModEnable = value
            );
            configMenu.AddKeybindList(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.mod_enable_toggle_key.name"),
                tooltip: () => this.Helper.Translation.Get("config.mod_enable_toggle_key.tooltip"),
                getValue: () => this.Config.ModEnableToggleKey,
                setValue: value => this.Config.ModEnableToggleKey = value
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.render_tree_trunk.name"),
                tooltip: () => this.Helper.Translation.Get("config.render_tree_trunk.tooltip"),
                getValue: () => this.Config.RenderTreeTrunk,
                setValue: value => this.Config.RenderTreeTrunk = value
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.render_leafy_shadow.name"),
                tooltip: () => this.Helper.Translation.Get("config.render_leafy_shadow.tooltip"),
                getValue: () => this.Config.RenderLeafyShadow,
                setValue: value => this.Config.RenderLeafyShadow = value
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.minish_oak.name"),
                tooltip: () => this.Helper.Translation.Get("config.minish_oak.tooltip"),
                getValue: () => this.Config.MinishOak,
                setValue: value => { this.Config.MinishOak = value; TreePatch.ChangeMinishTreeType(TreeTypeEnum.Oak.Id, value); }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.minish_maple.name"),
                tooltip: () => this.Helper.Translation.Get("config.minish_maple.tooltip"),
                getValue: () => this.Config.MinishMaple,
                setValue: value => { this.Config.MinishMaple = value; TreePatch.ChangeMinishTreeType(TreeTypeEnum.Maple.Id, value); }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.minish_pine.name"),
                tooltip: () => this.Helper.Translation.Get("config.minish_pine.tooltip"),
                getValue: () => this.Config.MinishPine,
                setValue: value => { this.Config.MinishPine = value; TreePatch.ChangeMinishTreeType(TreeTypeEnum.Pine.Id, value); }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.minish_mahogany.name"),
                tooltip: () => this.Helper.Translation.Get("config.minish_mahogany.tooltip"),
                getValue: () => this.Config.MinishMahogany,
                setValue: value => { this.Config.MinishMahogany = value; TreePatch.ChangeMinishTreeType(TreeTypeEnum.Mahogany.Id, value); }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.minish_green_rain_type1.name"),
                tooltip: () => this.Helper.Translation.Get("config.minish_green_rain_type1.tooltip"),
                getValue: () => this.Config.MinishGreenRainType1,
                setValue: value => { this.Config.MinishGreenRainType1 = value; TreePatch.ChangeMinishTreeType(TreeTypeEnum.GreenRainType1.Id, value); }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.minish_green_rain_type2.name"),
                tooltip: () => this.Helper.Translation.Get("config.minish_green_rain_type2.tooltip"),
                getValue: () => this.Config.MinishGreenRainType2,
                setValue: value => { this.Config.MinishGreenRainType2 = value; TreePatch.ChangeMinishTreeType(TreeTypeEnum.GreenRainType2.Id, value); }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.minish_green_rain_type3.name"),
                tooltip: () => this.Helper.Translation.Get("config.minish_green_rain_type3.tooltip"),
                getValue: () => this.Config.MinishGreenRainType3,
                setValue: value => { this.Config.MinishGreenRainType3 = value; TreePatch.ChangeMinishTreeType(TreeTypeEnum.GreenRainType3.Id, value); }
            );
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.minish_mystic.name"),
                tooltip: () => this.Helper.Translation.Get("config.minish_mystic.tooltip"),
                getValue: () => this.Config.MinishMystic,
                setValue: value => { this.Config.MinishMystic = value; TreePatch.ChangeMinishTreeType(TreeTypeEnum.Mystic.Id, value); }
            );
        }

        private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (ShouldEnable(forInput: true))
            {
                if (Config.ModEnableToggleKey.JustPressed())
                {
                    Config.ModEnable = !Config.ModEnable;
                    this.Helper.WriteConfig(this.Config);
                }
            }
        }

        private bool ShouldEnable(bool forInput = false)
        {
            if (!Context.IsWorldReady || !Context.IsMainPlayer)
            {
                return false;
            }
            if (forInput)
            {
                if (!Context.IsPlayerFree && !Game1.eventUp)
                {
                    return false;
                }
                if (Game1.keyboardDispatcher.Subscriber != null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}