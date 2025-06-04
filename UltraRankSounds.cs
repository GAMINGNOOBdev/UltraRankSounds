using System;
using BepInEx;
using System.IO;
using HarmonyLib;
using UnityEngine;
using PluginConfig.API;
using System.Reflection;
using PluginConfig.API.Fields;
using UltraRankSounds.Components;
using PluginConfig.API.Decorators;
using PluginConfig.API.Functionals;
using BepInEx.Logging;

namespace UltraRankSounds
{

    [BepInProcess("ULTRAKILL.exe")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class UltraRankSounds : BaseUnityPlugin
    {
        private static ManualLogSource logger;

        public static FloatSliderField VolumeSlider;
        public static ButtonField OpenSoundsFolder;
        public static BoolField PlayIfDescended;
        public static BoolField EnableSounds;

        private static PluginConfigurator config;

        private static void InitConfig()
        {
            config = PluginConfigurator.Create(PluginInfo.NAME, PluginInfo.GUID);
            config.SetIconWithURL("https://raw.githubusercontent.com/GAMINGNOOBdev/UltraRankSounds/refs/heads/master/icon.png");
            new ConfigHeader(config.rootPanel, "Sound settings");
            EnableSounds = new BoolField(config.rootPanel, "Enable rank sounds", "ultraranksounds.enabled", true);
            PlayIfDescended = new BoolField(config.rootPanel, "Play sound when rank descended", "ultraranksounds.playdescended", false);

            VolumeSlider = new FloatSliderField(config.rootPanel, "Volume", "ultraranksounds.volume", new Tuple<float, float>(0.0f, 1.0f), 1.0f, 2);

            new ConfigHeader(config.rootPanel, "Sound files");
            OpenSoundsFolder = new ButtonField(config.rootPanel, "Open sounds folder", "ultraranksounds.opensoundsfolder");

            EnableSounds.onValueChange += ChangedModStatus;
            VolumeSlider.onValueChange += VolumeChanged;
            OpenSoundsFolder.onClick += OpenDefaultSoundsFolder;

            EnableSounds.TriggerValueChangeEvent();
            VolumeSlider.TriggerValueChangeEvent();
        }

        private static void ChangedModStatus(BoolField.BoolValueChangeEvent e)
        {
            PlayIfDescended.interactable = e.value;
            VolumeSlider.interactable = e.value;
        }

        private static void VolumeChanged(FloatSliderField.FloatSliderValueChangeEvent e)
        {
            CustomSoundPlayer.Instance?.SetSoundVolume(e.newValue);
        }

        private static void OpenDefaultSoundsFolder()
        {
            Application.OpenURL(new Uri(SoundsConfig.DefaultSoundFolder).AbsoluteUri);
        }

        private void Awake()
        {
            logger = Logger;
            SoundsConfig.EnsureSoundDirectories();
            InitConfig();

            Harmony _patcher = new Harmony(PluginInfo.GUID);
            _patcher.PatchAll();

            Log($"Sound parent folder: '{SoundsConfig.DefaultSoundParentFolder}'");
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