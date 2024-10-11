using System;
using BepInEx;
using System.IO;
using HarmonyLib;
using PluginConfig.API;
using System.Reflection;
using PluginConfig.API.Fields;
using UltraRankSounds.Components;
using PluginConfig.API.Decorators;
using PluginConfig.API.Functionals;
using UnityEngine;

namespace UltraRankSounds
{

    [BepInProcess("ULTRAKILL.exe")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class UltraRankSounds : BaseUnityPlugin
    {
        public static string DefaultSoundFolder = $"{Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "sounds")}";

        public static string DestructiveSound   = $"{Path.Combine(DefaultSoundFolder!, "D.mp3")}";
        public static string ChaoticSound       = $"{Path.Combine(DefaultSoundFolder!, "C.mp3")}";
        public static string BrutalSound        = $"{Path.Combine(DefaultSoundFolder!, "B.mp3")}";
        public static string AnarchicSound      = $"{Path.Combine(DefaultSoundFolder!, "A.mp3")}";
        public static string SupremeSound       = $"{Path.Combine(DefaultSoundFolder!, "S.mp3")}";
        public static string SSadisticSound     = $"{Path.Combine(DefaultSoundFolder!, "SS.mp3")}";
        public static string SSShitstormSound   = $"{Path.Combine(DefaultSoundFolder!, "SSS.mp3")}";
        public static string ULTRAKILLSound     = $"{Path.Combine(DefaultSoundFolder!, "ULTR.mp3")}";

        public static FloatSliderField VolumeSlider;
        public static ButtonField OpenSoundsFolder;
        public static BoolField PlayIfDescended;
        public static BoolField EnableSounds;

        private static PluginConfigurator config;

        private static void InitConfig()
        {
            config = PluginConfigurator.Create(PluginInfo.NAME, PluginInfo.GUID);
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
            Application.OpenURL(new Uri(DefaultSoundFolder).AbsoluteUri);
        }

        private void Awake()
        {
            if (!Directory.Exists(DefaultSoundFolder))
                Directory.CreateDirectory(DefaultSoundFolder);

            InitConfig();

            Harmony _patcher = new Harmony(PluginInfo.GUID);
            _patcher.PatchAll();
        }
    }

}