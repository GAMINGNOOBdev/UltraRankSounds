using HarmonyLib;
using UnityEngine;
using UltraRankSounds.Components;

namespace UltraRankSounds.Patches
{

    [HarmonyPatch(typeof(StyleHUD))]
    public class StyleHUDPatch
    {
        private static CustomSoundPlayer customSoundPlayer;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StyleHUD), "Start")]
        public static void StyleHUD_Start_Postfix(GameObject ___styleHud, StyleHUD __instance)
        {
            customSoundPlayer = ___styleHud.AddComponent<CustomSoundPlayer>();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StyleHUD), "ComboStart")]
        public static void StyleHUD_ComboStart_Postfix()
        {
            PlaySoundForRank(0, "destructive (hooraaay!!!)");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StyleHUD), "AscendRank")]
        public static void StyleHUD_AscendRank_Postfix(StyleHUD __instance)
        {
            int rankIndex = __instance.rankIndex;
            string rankName = __instance.currentRank.sprite.name;

            PlaySoundForRank(rankIndex, rankName);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StyleHUD), "DescendRank")]
        public static void StyleHUD_DescendRank_Postfix(bool ___comboActive, StyleHUD __instance)
        {
            if (!UltraRankSounds.PlayIfDescended.value || !___comboActive)
                return;

            int rankIndex = __instance.rankIndex;
            string rankName = __instance.currentRank.sprite.name;

            PlaySoundForRank(rankIndex, rankName);
        }

        private static void PlaySoundForRank(int index, string name, bool ascended = true)
        {
            if (!UltraRankSounds.EnableSounds.value)
                return;

            string soundFile = GetSoundPathForRankIndex(index);
            string status = ascended ? "ascended" : "descended";

            Debug.Log($"{status} style rank to {name} [index: {index}] | playing sound {soundFile}");
            customSoundPlayer.PlaySound(soundFile);
        }

        private static string GetSoundPathForRankIndex(int rank)
        {
            if (rank == 0)
                return UltraRankSounds.DestructiveSound;

            if (rank == 1)
                return UltraRankSounds.ChaoticSound;

            if (rank == 2)
                return UltraRankSounds.BrutalSound;

            if (rank == 3)
                return UltraRankSounds.AnarchicSound;

            if (rank == 4)
                return UltraRankSounds.SupremeSound;

            if (rank == 5)
                return UltraRankSounds.SSadisticSound;

            if (rank == 6)
                return UltraRankSounds.SSShitstormSound;

            if (rank == 7)
                return UltraRankSounds.ULTRAKILLSound;

            return "";
        }

    }

}