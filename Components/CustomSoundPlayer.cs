using System;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace UltraRankSounds.Components
{

    public class CustomSoundPlayer : MonoBehaviour
    {
        private static CustomSoundPlayer _instance = null;
        public static CustomSoundPlayer Instance
        {
            get { return _instance; }
            set { throw new NotImplementedException(); }
        }

        public AudioSource source;
        private float soundVolume;
        private string soundPath;

        private void Start()
        {
            _instance = this;
            source = gameObject.AddComponent<AudioSource>();
            SetSoundVolume(UltraRankSounds.VolumeSlider.value);
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
            UltraRankSounds.Log($"Playing sound file '{file}'");
        }

        private IEnumerator PlaySoundRoutine()
        {
            WaitUntil songFinished = new(() => Application.isFocused && !source.isPlaying);

            FileInfo fileInfo = new(soundPath);
            AudioType audioType = CustomMusicFileBrowser.extensionTypeDict[fileInfo.Extension.ToLower()];

            using UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(new Uri(soundPath).AbsoluteUri, audioType);
            DownloadHandlerAudioClip handler = request.downloadHandler as DownloadHandlerAudioClip;
            handler.streamAudio = false;
            request.SendWebRequest();
            yield return request;

            source.clip = handler.audioClip;
            source.volume = soundVolume;
            source.Play();
            yield return songFinished;
            gameObject.SetActive(false);
            UnityEngine.Object.Destroy(handler.audioClip);
        }
        
    }

}