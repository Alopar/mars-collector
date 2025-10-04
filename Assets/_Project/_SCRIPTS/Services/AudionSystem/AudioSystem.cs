using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Services.AudionSystem
{
    public class AudioSystem : MonoBehaviour
    {
        #region FIELDS PRIVATE
        private AudioProvider _audioProvider;

        private AudioClip _nextMusicClip;
        private Coroutine _musicFadeCoroutine;
        #endregion

        #region METHODS PUBLIC
        public void Initialize(AudioProvider audioProvider)
        {
            _audioProvider = audioProvider;
        }

        public void PlayMusic(AudioClip clip)
        {
            if (!clip) return;
            if (!CheckAudioProviderStatus()) return;
            if (_audioProvider.MusicSource.isPlaying)
            {
                _nextMusicClip = clip;
                if (_musicFadeCoroutine != null)
                {
                     StopCoroutine(_musicFadeCoroutine);
                }

                _musicFadeCoroutine = StartCoroutine(FadeOutMusicAndSwitch());
            }
            else
            {
                _audioProvider.MusicSource.clip = clip;
                _audioProvider.MusicSource.Play();
            }
        }

        public void PlaySFX(AudioClip clip)
        {
            if (!clip) return;
            if (!CheckAudioProviderStatus()) return;
            _audioProvider.SoundSource.PlayOneShot(clip);
        }

        public Action PlayLoopingSFX(AudioClip clip)
        {
            Action cancellationToken = () => { /* Do nothing */ };

            if (!clip) return cancellationToken;
            if (!CheckAudioProviderStatus()) return cancellationToken;

            var source = _audioProvider.GetLoopingSource();
            source.clip = clip;
            source.Play();

            cancellationToken = () => source.Stop();
            return cancellationToken;
        }

        public void PlayUI(AudioClip clip)
        {
            if (!clip) return;
            if (!CheckAudioProviderStatus()) return;
            _audioProvider.ScreenSource.PlayOneShot(clip);
        }

        public AudioClip GetAudioClipByKey(string key)
        {
            if (!CheckAudioProviderStatus()) return null;
            if (!key.Contains("."))
            {
                Debug.LogError($"Invalid audio key format '{key}'. Expected format: 'mapKey.clipKey'");
                return null;
            }

            var parts = Regex.Match(key, @"^([^.]+)\.(.+)$");
            if (!parts.Success)
            {
                Debug.LogError($"Audio clip with key '{key}' not found");
                return null;
            }

            var mapKey = parts.Groups[1].Value;
            var clipKey = parts.Groups[2].Value;

            var audioMap = _audioProvider.AudioMaps.FirstOrDefault(map => map.Name == mapKey);
            if (!audioMap)
            {
                Debug.LogError($"Audio map with key '{mapKey}' not found");
                return null;
            }
            
            var clip = audioMap.Clips.FirstOrDefault(item => item.Key == clipKey)?.Clip;
            if (!clip)
            {
                Debug.LogError($"Audio clip with key '{clipKey}' not found in map '{mapKey}'");
                return null;
            }

            return clip;
        }

        public void SetMusicVolume(float volume)
        {
            if (!CheckAudioProviderStatus()) return;
            _audioProvider.MusicSource.volume = volume;
        }

        public void SetSoundVolume(float volume)
        {
            if (!CheckAudioProviderStatus()) return;
            _audioProvider.SoundSource.volume = volume;
            _audioProvider.ScreenSource.volume = volume;
            _audioProvider.LoopingSources.ForEach(source => source.volume = volume);
        }

        public void PauseMusic()
        {
            if (!CheckAudioProviderStatus()) return;
            if (!_audioProvider.MusicSource.isPlaying) return;

            _audioProvider.MusicSource.Pause();
            _audioProvider.LoopingSources.ForEach(source => source.Pause());
        }

        public void ResumeMusic()
        {
            if (!CheckAudioProviderStatus()) return;
            if (_audioProvider.MusicSource.clip == null) return;

            _audioProvider.MusicSource.UnPause();
            _audioProvider.LoopingSources.ForEach(source => source.UnPause());
        }
        #endregion

        #region METHODS PRIVATE
        private bool CheckAudioProviderStatus()
        {
            if (_audioProvider) return true;
            Debug.LogError("AudioProvider is not initialized");

            return false;
        }

        private IEnumerator FadeOutMusicAndSwitch()
        {
            var startVolume = _audioProvider.MusicSource.volume;
            var t = 0f;
            while (t < _audioProvider.MusicFadeDuration)
            {
                t += Time.unscaledDeltaTime;
                _audioProvider.MusicSource.volume = Mathf.Lerp(startVolume, 0f, t / _audioProvider.MusicFadeDuration);
                yield return null;
            }

            _audioProvider.MusicSource.Stop();
            _audioProvider.MusicSource.clip = _nextMusicClip;
            _audioProvider.MusicSource.Play();
            StartCoroutine(FadeInMusic(startVolume));
        }

        private IEnumerator FadeInMusic(float targetVolume)
        {
            var t = 0f;
            _audioProvider.MusicSource.volume = 0f;
            while (t < _audioProvider.MusicFadeDuration)
            {
                t += Time.unscaledDeltaTime;
                _audioProvider.MusicSource.volume = Mathf.Lerp(0f, targetVolume, t / _audioProvider.MusicFadeDuration);
                yield return null;
            }
            _audioProvider.MusicSource.volume = targetVolume;
        }
        #endregion
    }
}
