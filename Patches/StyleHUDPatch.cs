using HarmonyLib;
using UnityEngine;
using UltraRankSounds.Components;
using BepInEx;

namespace UltraRankSounds.Patches
{

    [HarmonyPatch(typeof(StyleHUD))]
    public class StyleHUDPatch
    {
        private static CustomSoundPlayer customSoundPlayer = null;
        private static int lastRankIndex = 0;

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
            if (!UltraRankSounds.EnableSounds.value || !UltraRankSounds.PlayIfDescended.value)
                return;

            PlaySoundForRank(0, "RankD");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StyleHUD), "AscendRank")]
        public static void StyleHUD_AscendRank_Postfix(StyleHUD __instance)
        {
            int rankIndex = __instance.rankIndex;
            string rankName = __instance.currentRank.sprite.name;

            PlaySoundForRank(rankIndex, rankName);
            lastRankIndex = rankIndex;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StyleHUD), "DescendRank")]
        public static void StyleHUD_DescendRank_Postfix(bool ___comboActive, StyleHUD __instance)
        {
            if (!UltraRankSounds.EnableSounds.value || !UltraRankSounds.PlayIfDescended.value || !___comboActive)
                return;

            int rankIndex = __instance.rankIndex;
            string rankName = __instance.currentRank.sprite.name;

            UltraRankSounds.Log($"style rank [index: {rankIndex} oldindex: {lastRankIndex}] | name '{rankName}'");

            PlaySoundForRank(rankIndex, rankName, false);
            lastRankIndex = rankIndex;
        }

        private static void PlaySoundForRank(int index, string name, bool ascended = true)
        {
            if (!UltraRankSounds.EnableSounds.value || customSoundPlayer == null)
                return;

            string soundFile = GetSoundPathForRankIndex(index, ascended);
            string status = ascended ? "ascended" : "descended";

            UltraRankSounds.Log($"{status} style rank to '{name}' [index: {index} oldindex: {lastRankIndex}] | playing sound '{soundFile}'");
            customSoundPlayer.PlaySound(soundFile);
        }

        private static string GetSoundPathForRankIndex(int rank, bool ascended)
        {
            int r = rank;

            if (ascended)
                return SoundsConfig.GetAscensionRankSoundName(r);

            return SoundsConfig.GetDescensionRankSoundName(r);
        }

    }

}