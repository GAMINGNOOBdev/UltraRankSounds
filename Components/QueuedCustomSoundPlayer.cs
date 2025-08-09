using System;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace UltraRankSounds.Components
{

    public class QueuedCustomSoundPlayer : MonoBehaviour
    {
        private static readonly List<QueuedCustomSoundPlayer> instances = [];
        public static void SetSoundVolumes(float volume)
        {
            foreach (QueuedCustomSoundPlayer csp in instances)
                csp.SetSoundVolume(volume);
        }

        public static void ClearAllSounds()
        {
            foreach (QueuedCustomSoundPlayer csp in instances)
                csp.sounds.Clear();
        }

        private readonly ConcurrentQueue<string> sounds = [];
        private AudioSource source;
        private float soundVolume;

        private void Start()
        {
            instances.Add(this);
            source = gameObject.AddComponent<AudioSource>();
            SetSoundVolumes(1);
            UltraRankSounds.MasterVolumeSlider.TriggerValueChangeEvent();
        }

        private void FixedUpdate()
        {
            if (source.isPlaying)
                return;
            
            if (sounds.IsEmpty)
                return;

            StartCoroutine(PlaySoundRoutine());
        }

        public void SetSoundVolume(float volume)
        {
            soundVolume = volume;
        }

        public void QueueSound(string file)
        {
            if (string.IsNullOrEmpty(file))
                return;

            if (!File.Exists(file))
            {
                UltraRankSounds.Log($"Could not find audio file '{file}'", true);
                return;
            }

            sounds.Enqueue(file);
            gameObject.SetActive(true);
            List<string> snds = [];
            foreach (var snd in sounds)
                snds.Add(Path.GetFileNameWithoutExtension(snd));
            UltraRankSounds.Log(string.Join(",", snds));
        }

        private IEnumerator PlaySoundRoutine()
        {
            string soundPath = null;
            while (!sounds.TryDequeue(out soundPath) && !sounds.IsEmpty);
            if (soundPath == null)
                yield break;
            WaitUntil soundFinished = new(() => Application.isFocused && !source.isPlaying);

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
            yield return soundFinished;
            Destroy(handler.audioClip);
        }

    }

}