using System;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace UltraRankSounds.Components
{

    public class CustomSoundPlayer : MonoBehaviour
    {
        private static readonly List<CustomSoundPlayer> instances = [];
        public static void SetSoundVolumes(float volume)
        {
            foreach (CustomSoundPlayer csp in instances)
                csp.SetSoundVolume(volume);
        }

        public AudioSource source;
        private float soundVolume;
        private string soundPath;

        private void Start()
        {
            instances.Add(this);
            source = gameObject.AddComponent<AudioSource>();
            SetSoundVolume(1);
            UltraRankSounds.MasterVolumeSlider.TriggerValueChangeEvent();
        }

        public void SetSoundVolume(float volume)
        {
            soundVolume = volume;
        }

        public void PlaySound(string file)
        {
            if (string.IsNullOrEmpty(file))
                return;

            if (!File.Exists(file))
            {
                UltraRankSounds.Log($"Could not find audio file '{file}'", true);
                return;
            }

            soundPath = file;
            gameObject.SetActive(true);
            StartCoroutine(PlaySoundRoutine());
        }

        private IEnumerator PlaySoundRoutine()
        {
            WaitUntil soundFinished = new(() => Application.isFocused && !source.isPlaying);

            FileInfo fileInfo = new(soundPath);
            AudioType audioType = CustomMusicFileBrowser.extensionTypeDict[fileInfo.Extension.ToLower()];

            using UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(new Uri(soundPath).AbsoluteUri, audioType);
            DownloadHandlerAudioClip handler = request.downloadHandler as DownloadHandlerAudioClip;
            handler.streamAudio = false;
            request.SendWebRequest();
            yield return request;

            source.PlayOneShot(handler.audioClip, soundVolume);
            yield return soundFinished;
            gameObject.SetActive(false);
            Destroy(handler.audioClip);
        }
        
    }

}