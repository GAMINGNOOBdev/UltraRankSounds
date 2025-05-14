using System.IO;
using System.Reflection;

public class SoundsConfig
{
    public static string DefaultSoundParentFolder = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}";
    public static string DefaultSoundFolder = $"{Path.Combine(DefaultSoundParentFolder!, "sounds")}";

    public class Uprank
    {
        public static string DestructiveSound   = $"{Path.Combine(DefaultSoundFolder!, "D.mp3")}";
        public static string ChaoticSound       = $"{Path.Combine(DefaultSoundFolder!, "C.mp3")}";
        public static string BrutalSound        = $"{Path.Combine(DefaultSoundFolder!, "B.mp3")}";
        public static string AnarchicSound      = $"{Path.Combine(DefaultSoundFolder!, "A.mp3")}";
        public static string SupremeSound       = $"{Path.Combine(DefaultSoundFolder!, "S.mp3")}";
        public static string SSadisticSound     = $"{Path.Combine(DefaultSoundFolder!, "SS.mp3")}";
        public static string SSShitstormSound   = $"{Path.Combine(DefaultSoundFolder!, "SSS.mp3")}";
        public static string ULTRAKILLSound     = $"{Path.Combine(DefaultSoundFolder!, "ULTR.mp3")}";
    }

    public class Downrank
    {
        public static string DestructiveSound   = $"{Path.Combine(DefaultSoundFolder!, "downrank-D.mp3")}";
        public static string ChaoticSound       = $"{Path.Combine(DefaultSoundFolder!, "downrank-C.mp3")}";
        public static string BrutalSound        = $"{Path.Combine(DefaultSoundFolder!, "downrank-B.mp3")}";
        public static string AnarchicSound      = $"{Path.Combine(DefaultSoundFolder!, "downrank-A.mp3")}";
        public static string SupremeSound       = $"{Path.Combine(DefaultSoundFolder!, "downrank-S.mp3")}";
        public static string SSadisticSound     = $"{Path.Combine(DefaultSoundFolder!, "downrank-SS.mp3")}";
        public static string SSShitstormSound   = $"{Path.Combine(DefaultSoundFolder!, "downrank-SSS.mp3")}";
    }

    public static string GetAscensionRankSoundName(int rank)
    {
        if (rank == 0)
            return Uprank.DestructiveSound;

        if (rank == 1)
            return Uprank.ChaoticSound;

        if (rank == 2)
            return Uprank.BrutalSound;

        if (rank == 3)
            return Uprank.AnarchicSound;

        if (rank == 4)
            return Uprank.SupremeSound;

        if (rank == 5)
            return Uprank.SSadisticSound;

        if (rank == 6)
            return Uprank.SSShitstormSound;

        if (rank == 7)
            return Uprank.ULTRAKILLSound;
        
        return "";
    }

    public static string GetDescensionRankSoundName(int rank)
    {
        if (rank == 0)
            return Downrank.DestructiveSound;

        if (rank == 1)
            return Downrank.ChaoticSound;

        if (rank == 2)
            return Downrank.BrutalSound;

        if (rank == 3)
            return Downrank.AnarchicSound;

        if (rank == 4)
            return Downrank.SupremeSound;

        if (rank == 5)
            return Downrank.SSadisticSound;

        if (rank == 6)
            return Downrank.SSShitstormSound;

        return "";
    }

    public static void EnsureSoundDirectories()
    {
        ///NOTE: this funcion only exists because r2modman once didn't copy over the right files

        if (!Directory.Exists(DefaultSoundFolder))
                Directory.CreateDirectory(DefaultSoundFolder);

        if (!File.Exists(Uprank.DestructiveSound)) // probably the stupidity of the mod manager, great zip extraction
        {
            // no i will not check every single file because if the mod manager did it for one file...
            // it will do it for the others as well

            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "D.mp3")}", Uprank.DestructiveSound);
            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "C.mp3")}", Uprank.ChaoticSound);
            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "B.mp3")}", Uprank.BrutalSound);
            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "A.mp3")}", Uprank.AnarchicSound);
            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "S.mp3")}", Uprank.SupremeSound);
            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "SS.mp3")}", Uprank.SSadisticSound);
            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "SSS.mp3")}", Uprank.SSShitstormSound);
            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "ULTR.mp3")}", Uprank.ULTRAKILLSound);

            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "downrank-D.mp3")}", Downrank.DestructiveSound);
            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "downrank-C.mp3")}", Downrank.ChaoticSound);
            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "downrank-B.mp3")}", Downrank.BrutalSound);
            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "downrank-A.mp3")}", Downrank.AnarchicSound);
            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "downrank-S.mp3")}", Downrank.SupremeSound);
            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "downrank-SS.mp3")}", Downrank.SSadisticSound);
            File.Move($"{Path.Combine(DefaultSoundParentFolder!, "downrank-SSS.mp3")}", Downrank.SSShitstormSound);
        }

    }
}