using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
namespace UltraRankSounds.Management;

public class SoundPack
{
    private readonly StyleRankSoundCollection downrankSounds, uprankSounds;
    private readonly Dictionary<string,string> styleBonuses;

    public string StyleRanksFolder
    {
        get;
        private set;
    }

    public string StyleBonusesFolder
    {
        get;
        set;
    }

    public SoundPack(string packFolderPath)
    {
        StyleRanksFolder = packFolderPath;
        StyleBonusesFolder = $"{Path.Combine(StyleRanksFolder!, "stylebonuses")}";

        styleBonuses = [];
        uprankSounds = new(null, "downrank-");
        downrankSounds = new("downrank-");
    }

    public string GetAscensionRankSoundName(int rank) => uprankSounds.DecideSound(rank);
    public string GetDescensionRankSoundName(int rank) => downrankSounds.DecideSound(rank);

    public string GetPointBonusSound(string id)
    {
        if (!styleBonuses.ContainsKey(id))
            return null;

        return styleBonuses[id];
    }

    public void UpdateSoundEntries()
    {
        string[] styleRankFiles = Directory.GetFiles(StyleRanksFolder, "*", SearchOption.TopDirectoryOnly);
        if (styleRankFiles.Length == 0)
        {
            UltraRankSounds.Log($"Cannot find any sound files for soundpack at '{StyleRanksFolder}'", true);
        }
        else
        {
            uprankSounds.RegisterSounds(ref styleRankFiles);
            downrankSounds.RegisterSounds(ref styleRankFiles);
        }

        styleBonuses.Clear();
        if (!Directory.Exists(StyleBonusesFolder))
        {
            UltraRankSounds.Log($"no style bonus directory ('{StyleBonusesFolder}')");
            return;
        }

        string[] styleBonusFiles = Directory.GetFiles(StyleBonusesFolder, "*", SearchOption.TopDirectoryOnly);
        UltraRankSounds.Log($"found the desired style bonus directory with {styleBonusFiles.Length} items ('{StyleBonusesFolder}')");
        foreach (string file in styleBonusFiles)
        {
            string filename = Path.GetFileNameWithoutExtension(file);
            styleBonuses.Add(filename, file);
        }

        UltraRankSounds.Log($"Registered style bonuses '{string.Join(",", [.. styleBonuses.Keys.ToArray()])}'");
    }

    public void Install(string zipPath)
    {
        Directory.CreateDirectory(StyleRanksFolder);
        Directory.CreateDirectory(StyleBonusesFolder);
        ZipFile.ExtractToDirectory(zipPath, StyleRanksFolder, true);
    }

    public void Export(string zipPath) => ZipFile.CreateFromDirectory(StyleRanksFolder, zipPath, CompressionLevel.NoCompression, false);
}
