using System.Collections.Generic;
namespace UltraRankSounds.Management;

public class SoundCollection
{
    private int cursor = 0;

    public List<string> Sounds
    {
        get;
        private set;
    }

    public SoundCollection()
    {
        Sounds = [];
    }

    public void AddSound(string soundfile)
    {
        Sounds.Add(soundfile);
    }

    public void Clear()
    {
        Sounds.Clear();
        cursor = 0;
    }

    public string DecideSound()
    {
        if (Sounds.Count == 0)
            return "none";

        if (UltraRankSounds.PlaySoundsInLoadedOrder.value)
        {
            cursor++;
            cursor %= Sounds.Count;
            return Sounds[cursor];
        }

        return Sounds[UltraRankSounds.randomNumGen.Next(0, Sounds.Count)];
    }
}
