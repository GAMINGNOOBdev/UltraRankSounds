using System.IO;
namespace UltraRankSounds.Management;

public class StyleRankSoundCollection
{
    private readonly string prefix = null;
    private readonly string discardPrefix = null;

    public SoundCollection DestructiveSound = null;
    public SoundCollection ChaoticSound = null;
    public SoundCollection BrutalSound = null;
    public SoundCollection AnarchicSound = null;
    public SoundCollection SupremeSound = null;
    public SoundCollection SSadisticSound = null;
    public SoundCollection SSShitstormSound = null;
    public SoundCollection ULTRAKILLSound = null;

    public StyleRankSoundCollection(string namePrefix, string discardNamePrefix = null)
    {
        DestructiveSound = new();
        ChaoticSound = new();
        BrutalSound = new();
        AnarchicSound = new();
        SupremeSound = new();
        SSadisticSound = new();
        SSShitstormSound = new();
        ULTRAKILLSound = new();
        prefix = namePrefix;
        discardPrefix = discardNamePrefix;
        prefix ??= "";
    }

    public void RegisterSounds(ref string[] files)
    {
        DestructiveSound.Clear();
        ChaoticSound.Clear();
        BrutalSound.Clear();
        AnarchicSound.Clear();
        SupremeSound.Clear();
        SSadisticSound.Clear();
        SSShitstormSound.Clear();
        ULTRAKILLSound.Clear();

        foreach (string filepath in files)
        {
            string file = Path.GetFileName(filepath);
            if (discardPrefix != null && file.StartsWith(discardPrefix))
                continue;
            if (!file.StartsWith(prefix))
                continue;

            file = file[prefix.Length..];

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

        UltraRankSounds.Log("Loaded Sounds");
        UltraRankSounds.Log($"ULTRAKILL '{string.Join(",", [.. ULTRAKILLSound.Sounds])}'");
        UltraRankSounds.Log($"SSShitstorm '{string.Join(",", [.. SSShitstormSound.Sounds])}'");
        UltraRankSounds.Log($"SSadistic '{string.Join(",", [.. SSadisticSound.Sounds])}'");
        UltraRankSounds.Log($"Supreme '{string.Join(",", [.. SupremeSound.Sounds])}'");
        UltraRankSounds.Log($"Destructive '{string.Join(",", [.. DestructiveSound.Sounds])}'");
        UltraRankSounds.Log($"Chaotic '{string.Join(",", [.. ChaoticSound.Sounds])}'");
        UltraRankSounds.Log($"Brutal '{string.Join(",",BrutalSound.Sounds.ToArray())}'");
        UltraRankSounds.Log($"Anarchic '{string.Join(",",AnarchicSound.Sounds.ToArray())}'");
    }

    public string DecideSound(int rank)
    {
        if (rank == SoundRanks.DESTRUCTIVE)
            return DestructiveSound.DecideSound();
        else if (rank == SoundRanks.CHAOTIC)
            return ChaoticSound.DecideSound();
        else if (rank == SoundRanks.BRUTAL)
            return BrutalSound.DecideSound();
        else if (rank == SoundRanks.ANARCHIC)
            return AnarchicSound.DecideSound();
        else if (rank == SoundRanks.SUPREME)
            return SupremeSound.DecideSound();
        else if (rank == SoundRanks.SSADISTIC)
            return SSadisticSound.DecideSound();
        else if (rank == SoundRanks.SSSHITSTORM)
            return SSShitstormSound.DecideSound();
        else if (rank == SoundRanks.ULTRAKILL)
            return ULTRAKILLSound.DecideSound();

        return "";
    }
}