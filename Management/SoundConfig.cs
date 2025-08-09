using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
namespace UltraRankSounds.Management;

public class SoundConfig
{
    public static readonly string defaultSoundParentFolder = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}";
    public static readonly string defaultSoundPacksFolder = $"{Path.Combine(defaultSoundParentFolder!, "soundpacks")}";

    private static readonly SoundPack defaultSoundPack = new($"{Path.Combine(defaultSoundParentFolder!, "sounds")}");
    public static readonly Dictionary<string,SoundPack> SoundPacks = [];
    public static SoundPack CurrentSoundPack = defaultSoundPack;

    public static void RefreshEntries()
    {
        string value = UltraRankSounds.CurrentSoundPack.value;
        foreach (var _ in SoundPacks.Keys)
            UltraRankSounds.CurrentSoundPack.RemoveAt(1);

        LoadSoundPacks();

        foreach (string i in SoundPacks.Keys)
            UltraRankSounds.CurrentSoundPack.AddValue(i);
        
        if (SoundPacks.ContainsKey(value))
            UltraRankSounds.CurrentSoundPack.value = value;

        UpdateSoundEntries();
    }

    public static void LoadSoundPacks()
    {
        SoundPacks.Clear();

        if (!Directory.Exists(defaultSoundPacksFolder))
        {
            Directory.CreateDirectory(defaultSoundPacksFolder);
            return;
        }

        string[] packs = Directory.GetDirectories(defaultSoundPacksFolder, "*", new EnumerationOptions(){RecurseSubdirectories=false});
        if (packs.Length == 0)
            return;
        
        foreach (string pack in packs)
        {
            string packName = Path.GetFileName(pack);
            SoundPacks.Add(packName, new(pack));
        }
    }

    public static void SetSoundPack(string name)
    {
        if (name == "default")
        {
            CurrentSoundPack = defaultSoundPack;
            return;
        }

        if (!SoundPacks.ContainsKey(name))
            return;

        CurrentSoundPack = SoundPacks[name];
        UpdateSoundEntries();
        UltraRankSounds.Log($"Changed sound pack to '{name}'");
    }

    public static void InstallSoundPack(string path)
    {
        string packName = Path.GetFileNameWithoutExtension(path);
        SoundPack sp = new($"{Path.Combine(defaultSoundPacksFolder!, packName)}");
        sp.Install(path);
    }

    public static string GetPointBonusSound(string id) => CurrentSoundPack.GetPointBonusSound(id);

    public static string GetAscensionRankSoundName(int rank) => CurrentSoundPack.GetAscensionRankSoundName(rank);
    public static string GetDescensionRankSoundName(int rank) => CurrentSoundPack.GetDescensionRankSoundName(rank);
    public static void UpdateSoundEntries() => CurrentSoundPack.UpdateSoundEntries();
}
