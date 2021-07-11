using System;
using System.Collections.Generic;
using CustomExtension;
using GT.Asset;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace GT.Audio
{
    public class Audio : MonoBehaviour, IResourceReferenceHolder
    {
        public static float MusicVolume;
        public static float SoundsVolume;

        private List<AudioSource> _sfxAudioSources;
        private AudioSource _musicSource; // for now only one
        [SerializeField] private AudioListener audioListener;

        public int sfxSourcesLimit = 12;
        public AudioMixerGroup sfxAudioMixerGroup = null;
        public Action DestroyResource { get; set; }
        private Dictionary<SoundFx, AudioClip> _cachedClips = new Dictionary<SoundFx, AudioClip>();

        private void Awake()
        {
            Debug.Log(":: Init Audio");
            _sfxAudioSources = new List<AudioSource>();
        }

        private void PreCacheSfx()
        {
            StartCoroutine(
                AddressableHelper.LoadAssetsByEnum<SoundFx, AudioClip>(this, clips => { _cachedClips = clips; }));
        }

        public void Mute()
        {
        }

        public void UnMute()
        {
        }

        public void Shutdown()
        {
        }

        public void SetAudioListener(GameObject musicListenerObject)
        {
            if (audioListener != null && musicListenerObject != audioListener.gameObject)
            {
                Debug.LogWarning("Found another audio listener set before");
                return;
            }

            audioListener = musicListenerObject.GetOrAddComponent<AudioListener>();
        }

        public void SetMusicSource(GameObject musicSourceObject)
        {
            if (_musicSource != null && musicSourceObject != _musicSource.gameObject)
            {
                Debug.LogWarning("Found another music source set before");
                return;
            }

            _musicSource = musicSourceObject.GetOrAddComponent<AudioSource>();
        }

        private void AddNewAudioListener(GameObject parentObject, bool forceAdd)
        {
        }

        public void StopAllSfx()
        {
            _sfxAudioSources.ForEach(sfxSource => sfxSource.Stop());
        }

        public void PlaySfx(SoundFx soundFx, float volume = -1)
        {
            if (_cachedClips.TryGetValue(soundFx, out var c))
                PlaySfx(c);
            else
                Debug.LogError($"Audio address: {soundFx} don't exists.");
        }

        public void PlaySfxOnce<TK>(TK key, float volume = -1) where TK : struct
        {
            AddressableHelper.LoadAsset<AudioClip, TK>(key, (clip, handle) =>
            {
                PlaySfx(clip, volume);
                this.DelayCoroutine(clip.length, () => Addressables.Release(handle));
            });
        }

        public void PlaySfx(AudioClip audioClip, float volume = -1)
        {
            if (audioListener == false)
            {
                Debug.LogError("Didn't find the audio listener");
                return;
            }

            var availableSource = _sfxAudioSources.Find(audioSource => audioSource.isPlaying == false);
            if (availableSource == null)
            {
                if (_sfxAudioSources.Count < sfxSourcesLimit)
                {
                    availableSource = gameObject.AddComponent<AudioSource>();
                    _sfxAudioSources.Add(availableSource);
                }
                else
                {
                    availableSource = _sfxAudioSources.WhereMax(audioSource => audioSource.time);
                }
            }

            availableSource.volume = volume > 0 ? volume : 1.0f;

            availableSource.outputAudioMixerGroup = sfxAudioMixerGroup;
            availableSource.PlayOneShot(audioClip);
        }

        public void PlayBackgroundMusic(SoundFx soundFx, float volume = -1)
        {
            if (_cachedClips.TryGetValue(soundFx, out var c))
                PlayMusic(c, volume);
            else
                Debug.LogError($"Audio address: {soundFx} don't exists.");
        }

        private void PlayMusic(AudioClip audioClip, float volume = -1)
        {
            if (audioListener == false)
            {
                Debug.LogError("Didn't find the audio listener");
                return;
            }

            if (_musicSource == null)
            {
                _musicSource = gameObject.AddComponent<AudioSource>();
            }

            _musicSource.loop = true;
            _musicSource.volume = volume > 0 ? volume : 1.0f;
            _musicSource.clip = audioClip;
            _musicSource.Play();
        }

        private void OnDestroy()
        {
            DestroyResource?.Invoke();
        }
    }
}