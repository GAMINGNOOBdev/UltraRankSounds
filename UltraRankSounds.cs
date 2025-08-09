using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using BepInEx.Logging;
using PluginConfig.API;
using PluginConfig.API.Fields;
using UltraRankSounds.Components;
using System.Collections.Generic;
using PluginConfig.API.Decorators;
using PluginConfig.API.Functionals;
using SettingsMenu.Components;
using System.Linq;
using MonoMod.Utils;

namespace UltraRankSounds
{

    [BepInProcess("ULTRAKILL.exe")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class UltraRankSounds : BaseUnityPlugin
    {
        private static readonly Dictionary<string,string> currentAvailableStylePointBonuses = [];
        private static PluginConfigurator config;
        private static ManualLogSource logger;

        public static BoolField PlaySoundsInLoadedOrder, EnableStyleBonusSounds, PlayIfDescended, EnableSounds;
        public static FloatSliderField MasterVolumeSlider, StyleRankVolumeSlider, StyleBonusVolumeSlider;
        public static ButtonField RefreshFiles, OpenSoundPacksFolder, OpenSoundPack;
        public static StringMultilineField CurrentAvailableStylePointBonusesText;
        public static StringListField CurrentSoundPack;

        public static readonly System.Random randomNumGen = new();

        private static void InitConfig()
        {
            config = PluginConfigurator.Create(PluginInfo.NAME, PluginInfo.GUID);
            config.SetIconWithURL("https://raw.githubusercontent.com/GAMINGNOOBdev/UltraRankSounds/refs/heads/master/icon.png");
            new ConfigHeader(config.rootPanel, "Sound settings");
            EnableSounds = new(config.rootPanel, "Enable rank sounds", "ultraranksounds.enabled", true);
            PlayIfDescended = new(config.rootPanel, "Play sound when rank descended", "ultraranksounds.playdescended", true);
            PlaySoundsInLoadedOrder = new(config.rootPanel, "Play sounds in loaded order", "ultraranksounds.playinloadedorder", false);
            EnableStyleBonusSounds = new(config.rootPanel, "Enable sounds for style bonuses", "ultraranksounds.playsoundsforstylebonuses", false);
            List<string> soundPacks = ["default"];
            foreach (string i in Management.SoundConfig.SoundPacks.Keys)
                soundPacks.Add(i);
            CurrentSoundPack = new(config.rootPanel, "Selected sound pack", "ultraranksounds.currentsoundpack", soundPacks.ToArray(), "default");
            MasterVolumeSlider = new(config.rootPanel, "Volume", "ultraranksounds.volume", new Tuple<float, float>(0.0f, 1.0f), 1.0f, 2);
            StyleRankVolumeSlider = new(config.rootPanel, "Style rank volume", "ultraranksounds.stylerankvolume", new Tuple<float, float>(0.0f, 1.0f), 1.0f, 2);
            StyleBonusVolumeSlider = new(config.rootPanel, "Style bonus volume", "ultraranksounds.stylebonusvolume", new Tuple<float, float>(0.0f, 1.0f), 0.5f, 2);

            new ConfigHeader(config.rootPanel, "Management");
            OpenSoundPack = new(config.rootPanel, "Open sound pack", "ultraranksounds.opensoundpack");
            OpenSoundPacksFolder = new(config.rootPanel, "Open sound packs folder", "ultraranksounds.opensoundpacksfolder");
            RefreshFiles = new(config.rootPanel, "Refresh all files", "ultraranksounds.refreshall");

            new ConfigHeader(config.rootPanel, "Currently Available Style Point Bonuses");
            CurrentAvailableStylePointBonusesText = new(config.rootPanel, "", "ultraranksounds.curravailablestylebonuses", "", true, false)
            {
                interactable = false,
            };

            EnableSounds.onValueChange += (BoolField.BoolValueChangeEvent e) => {
                PlayIfDescended.interactable = e.value;
                PlaySoundsInLoadedOrder.interactable = e.value;
                EnableStyleBonusSounds.interactable = e.value;
                CurrentSoundPack.interactable = e.value;
                MasterVolumeSlider.interactable = e.value;
                StyleRankVolumeSlider.interactable = e.value;
                StyleBonusVolumeSlider.interactable = e.value;
            };
            CurrentSoundPack.onValueChange += (StringListField.StringListValueChangeEvent e) => {
                Management.SoundConfig.SetSoundPack(e.value);
                QueuedCustomSoundPlayer.ClearAllSounds();
            };
            MasterVolumeSlider.onValueChange += (FloatSliderField.FloatSliderValueChangeEvent e) =>
            {
                CustomSoundPlayer.SetSoundVolumes(e.newValue * StyleRankVolumeSlider.value);
                QueuedCustomSoundPlayer.SetSoundVolumes(e.newValue * StyleBonusVolumeSlider.value);
            };
            StyleRankVolumeSlider.onValueChange += (FloatSliderField.FloatSliderValueChangeEvent e) =>
            {
                CustomSoundPlayer.SetSoundVolumes(MasterVolumeSlider.value * e.newValue);
            };
            StyleBonusVolumeSlider.onValueChange += (FloatSliderField.FloatSliderValueChangeEvent e) =>
            {
                QueuedCustomSoundPlayer.SetSoundVolumes(MasterVolumeSlider.value * e.newValue);
            };
            OpenSoundPack.onClick += () => Application.OpenURL(new Uri(Management.SoundConfig.CurrentSoundPack.StyleRanksFolder).AbsoluteUri);
            OpenSoundPacksFolder.onClick += () => Application.OpenURL(new Uri(Management.SoundConfig.defaultSoundPacksFolder).AbsoluteUri);
            RefreshFiles.onClick += Management.SoundConfig.RefreshEntries;

            EnableSounds.TriggerValueChangeEvent();
            CurrentSoundPack.TriggerValueChangeEvent();
            MasterVolumeSlider.TriggerValueChangeEvent();
            StyleRankVolumeSlider.TriggerValueChangeEvent();
            StyleBonusVolumeSlider.TriggerValueChangeEvent();
        }

        private void Awake()
        {
            logger = Logger;
            Management.SoundConfig.LoadSoundPacks();
            Management.SoundConfig.UpdateSoundEntries();
            InitConfig();

            Harmony _patcher = new(PluginInfo.GUID);
            _patcher.PatchAll();

            Log($"Default sound pack folder: '{Management.SoundConfig.CurrentSoundPack.StyleRanksFolder}'");
            Log($"Default sound pack style bonus folder: '{Management.SoundConfig.CurrentSoundPack.StyleBonusesFolder}'");
            Log($"Sound packs folder: '{Management.SoundConfig.defaultSoundPacksFolder}'");
        }

        public static bool IsStyleBonus(string bonus)
        {
            return currentAvailableStylePointBonuses.ContainsKey(bonus);
        }

        public static void RegisterStylePointBonuses(Dictionary<string,string> styleBonusses)
        {
            if (styleBonusses == null)
            {
                Log("unable to register style point bonuses: no style bonuses dict found");
                return;
            }

            currentAvailableStylePointBonuses.Clear();

            FormattedStringBuilder formattedStringBuilder = new();
            foreach (string id in styleBonusses.Keys)
            {
                string name = styleBonusses[id];
                if (name.IsNullOrWhiteSpace())
                    continue;

                currentAvailableStylePointBonuses.Add(id, name);
                formattedStringBuilder.Append(name + " - " + id + "\n");
            }
            CurrentAvailableStylePointBonusesText.value = formattedStringBuilder.Build().rawString;
            CurrentAvailableStylePointBonusesText.TriggerValueChangeEvent();
        }

        public static void Log(string message, bool error = false)
        {
            if (logger == null)
                return;

            if (error)
            {
                logger.LogError(message);
                return;
            }

            logger.LogInfo(message);
        }

    }

}