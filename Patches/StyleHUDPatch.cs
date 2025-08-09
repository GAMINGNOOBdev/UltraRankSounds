using HarmonyLib;
using UnityEngine;
using UltraRankSounds.Components;
using BepInEx;
using System.Collections.Generic;

namespace UltraRankSounds.Patches
{

    [HarmonyPatch(typeof(StyleHUD))]
    public class StyleHUDPatch
    {
        private static CustomSoundPlayer styleRankSoundPlayer = null;
        private static QueuedCustomSoundPlayer styleBonusSoundPlayer = null;
        private static int lastRankIndex = 0;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StyleHUD), "Start")]
        public static void StyleHUD_Start_Postfix(GameObject ___styleHud, StyleHUD __instance)
        {
            styleRankSoundPlayer = ___styleHud.AddComponent<CustomSoundPlayer>();
            styleBonusSoundPlayer = ___styleHud.AddComponent<QueuedCustomSoundPlayer>();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StyleHUD), "Awake")]
        public static void StyleHUD_Awake_Postfix(Dictionary<string,string> ___idNameDict)
        {
            UltraRankSounds.RegisterStylePointBonuses(___idNameDict);
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
        [HarmonyPatch(typeof(StyleHUD), "AddPoints")]
        public static void StyleHUD_AddPoints_Postfix(string pointID, StyleHUD __instance)
        {
            if (!UltraRankSounds.EnableSounds.value || !UltraRankSounds.EnableStyleBonusSounds.value)
                return;

            string localizedName = __instance.GetLocalizedName(pointID);

            if (string.IsNullOrWhiteSpace(localizedName) || string.IsNullOrWhiteSpace(pointID))
                return;

            UltraRankSounds.Log($"got style bonus '{localizedName}' with id '{pointID}'");
            PlaySoundForBonus(pointID);
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
            if (!UltraRankSounds.EnableSounds.value || styleRankSoundPlayer == null)
                return;

            string soundFile = GetSoundPathForRankIndex(index, ascended);
            string status = ascended ? "ascended" : "descended";

            UltraRankSounds.Log($"{status} style rank to '{name}' [index: {index} oldindex: {lastRankIndex}] | playing sound '{soundFile}'");
            styleRankSoundPlayer.PlaySound(soundFile);
        }

        private static void PlaySoundForBonus(string id)
        {
            if (!UltraRankSounds.EnableSounds.value || styleBonusSoundPlayer == null)
                return;

            string soundFile = Management.SoundConfig.GetPointBonusSound(id);
            if (string.IsNullOrEmpty(soundFile))
            {
                UltraRankSounds.Log($"Unable to find rank sound for point bonus '{id}'", true);
                return;
            }

            UltraRankSounds.Log($"{id} style bonus registered | playing sound '{soundFile}'");
            styleBonusSoundPlayer.QueueSound(soundFile);
        }

        private static string GetSoundPathForRankIndex(int rank, bool ascended)
        {
            int r = rank;

            if (ascended)
                return Management.SoundConfig.GetAscensionRankSoundName(r);

            return Management.SoundConfig.GetDescensionRankSoundName(r);
        }

    }
}