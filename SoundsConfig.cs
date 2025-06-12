using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

public class SoundsConfig
{
    public static string DefaultSoundParentFolder = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}";
    public static string DefaultSoundFolder = $"{Path.Combine(DefaultSoundParentFolder!, "sounds")}";
    private static Random randomNumGen = new();
    private static int soundFileCount = 0;

    public class SoundCollection
    {
        private List<string> savedStrings = [];
        private int cursor = 0;

        public List<string> Sounds
        {
            get => savedStrings;
            set => throw new NotSupportedException();
        }

        public void AddSound(string soundfile)
        {
            savedStrings.Add(soundfile);
        }

        public void Clear()
        {
            savedStrings.Clear();
            cursor = 0;
        }

        public string DecideSound()
        {
            if (savedStrings.Count == 0)
                return "none";

            if (UltraRankSounds.UltraRankSounds.PlaySoundsInAlphabeticOrder.value)
            {
                cursor++;
                cursor %= savedStrings.Count;
                return savedStrings[cursor];
            }

            return savedStrings[randomNumGen.Next(0, savedStrings.Count)];
        }
    }

    public class Uprank
    {
        public static SoundCollection DestructiveSound = new();
        public static SoundCollection ChaoticSound = new();
        public static SoundCollection BrutalSound = new();
        public static SoundCollection AnarchicSound = new();
        public static SoundCollection SupremeSound = new();
        public static SoundCollection SSadisticSound = new();
        public static SoundCollection SSShitstormSound = new();
        public static SoundCollection ULTRAKILLSound = new();

        public static void RegisterSounds(string[] files)
        {
            foreach (string filepath in files)
            {
                string file = Path.GetFileName(filepath);
                if (file.StartsWith("downrank-"))
                    continue;

                UltraRankSounds.UltraRankSounds.Log($"found sound file '{file}'");
                if (file.StartsWith("ULTR"))
                    ULTRAKILLSound.AddSound(filepath);
                else if (file.StartsWith("SSS"))
                    SSShitstormSound.AddSound(filepath);
                else if (file.StartsWith("SS"))
                    SSadisticSound.AddSound(filepath);
                else if (file.StartsWith("S"))
                    SupremeSound.AddSound(filepath);
                else if (file.StartsWith("D"))
                    DestructiveSound.AddSound(filepath);
                else if (file.StartsWith("C"))
                    ChaoticSound.AddSound(filepath);
                else if (file.StartsWith("B"))
                    BrutalSound.AddSound(filepath);
                else if (file.StartsWith("A"))
                    AnarchicSound.AddSound(filepath);
            }

            UltraRankSounds.UltraRankSounds.Log("Uprank Sounds", false);
            UltraRankSounds.UltraRankSounds.Log($"ULTRAKILL '{string.Join(",",ULTRAKILLSound.Sounds.ToArray())}'", false);
            UltraRankSounds.UltraRankSounds.Log($"SSShitstorm '{string.Join(",",SSShitstormSound.Sounds.ToArray())}'", false);
            UltraRankSounds.UltraRankSounds.Log($"SSadistic '{string.Join(",",SSadisticSound.Sounds.ToArray())}'", false);
            UltraRankSounds.UltraRankSounds.Log($"Supreme '{string.Join(",",SupremeSound.Sounds.ToArray())}'", false);
            UltraRankSounds.UltraRankSounds.Log($"Destructive '{string.Join(",",DestructiveSound.Sounds.ToArray())}'", false);
            UltraRankSounds.UltraRankSounds.Log($"Chaotic '{string.Join(",",ChaoticSound.Sounds.ToArray())}'", false);
            UltraRankSounds.UltraRankSounds.Log($"Brutal '{string.Join(",",BrutalSound.Sounds.ToArray())}'", false);
            UltraRankSounds.UltraRankSounds.Log($"Anarchic '{string.Join(",",AnarchicSound.Sounds.ToArray())}'", false);
        }
    }

    public class Downrank
    {
        public static SoundCollection DestructiveSound = new();
        public static SoundCollection ChaoticSound = new();
        public static SoundCollection BrutalSound = new();
        public static SoundCollection AnarchicSound = new();
        public static SoundCollection SupremeSound = new();
        public static SoundCollection SSadisticSound = new();
        public static SoundCollection SSShitstormSound = new();

        public static void RegisterSounds(string[] files)
        {
            foreach (string filepath in files)
            {
                string file = Path.GetFileName(filepath);
                if (!file.StartsWith("downrank-"))
                    continue;

                if (file.StartsWith("downrank-SSS"))
                    SSShitstormSound.AddSound(filepath);
                else if (file.StartsWith("downrank-SS"))
                    SSadisticSound.AddSound(filepath);
                else if (file.StartsWith("downrank-S"))
                    SupremeSound.AddSound(filepath);
                else if (file.StartsWith("downrank-D"))
                    DestructiveSound.AddSound(filepath);
                else if (file.StartsWith("downrank-C"))
                    ChaoticSound.AddSound(filepath);
                else if (file.StartsWith("downrank-B"))
                    BrutalSound.AddSound(filepath);
                else if (file.StartsWith("downrank-A"))
                    AnarchicSound.AddSound(filepath);
            }

            UltraRankSounds.UltraRankSounds.Log("Downrank Sounds", false);
            UltraRankSounds.UltraRankSounds.Log($"SSShitstorm '{string.Join(",",SSShitstormSound.Sounds.ToArray())}'", false);
            UltraRankSounds.UltraRankSounds.Log($"SSadistic '{string.Join(",",SSadisticSound.Sounds.ToArray())}'", false);
            UltraRankSounds.UltraRankSounds.Log($"Supreme '{string.Join(",",SupremeSound.Sounds.ToArray())}'", false);
            UltraRankSounds.UltraRankSounds.Log($"Destructive '{string.Join(",",DestructiveSound.Sounds.ToArray())}'", false);
            UltraRankSounds.UltraRankSounds.Log($"Chaotic '{string.Join(",",ChaoticSound.Sounds.ToArray())}'", false);
            UltraRankSounds.UltraRankSounds.Log($"Brutal '{string.Join(",",BrutalSound.Sounds.ToArray())}'", false);
            UltraRankSounds.UltraRankSounds.Log($"Anarchic '{string.Join(",",AnarchicSound.Sounds.ToArray())}'", false);
        }
    }

    public static string GetAscensionRankSoundName(int rank)
    {
        if (rank == 0)
            return Uprank.DestructiveSound.DecideSound();

        if (rank == 1)
            return Uprank.ChaoticSound.DecideSound();

        if (rank == 2)
            return Uprank.BrutalSound.DecideSound();

        if (rank == 3)
            return Uprank.AnarchicSound.DecideSound();

        if (rank == 4)
            return Uprank.SupremeSound.DecideSound();

        if (rank == 5)
            return Uprank.SSadisticSound.DecideSound();

        if (rank == 6)
            return Uprank.SSShitstormSound.DecideSound();

        if (rank == 7)
            return Uprank.ULTRAKILLSound.DecideSound();
        
        return "";
    }

    public static string GetDescensionRankSoundName(int rank)
    {
        if (rank == 0)
            return Downrank.DestructiveSound.DecideSound();

        if (rank == 1)
            return Downrank.ChaoticSound.DecideSound();

        if (rank == 2)
            return Downrank.BrutalSound.DecideSound();

        if (rank == 3)
            return Downrank.AnarchicSound.DecideSound();

        if (rank == 4)
            return Downrank.SupremeSound.DecideSound();

        if (rank == 5)
            return Downrank.SSadisticSound.DecideSound();

        if (rank == 6)
            return Downrank.SSShitstormSound.DecideSound();

        return "";
    }

    public static void UpdateSoundEntries()
    {
        string[] files = Directory.GetFiles(DefaultSoundFolder);
        if (files.Length == 0)
        {
            UltraRankSounds.UltraRankSounds.Log("Cannot find any sound files", true);
            return;
        }

        if (files.Length == soundFileCount)
            return;

        Uprank.RegisterSounds(files);
        Downrank.RegisterSounds(files);

        soundFileCount = files.Length;
    }
}