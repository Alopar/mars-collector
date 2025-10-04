using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Services.AudionSystem
{
    public class AudioProvider : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [Header("AUDIO SETTINGS")]
        [SerializeField, Range(0, 5)] private float _musicFadeDuration = 1.5f;

        [Header("AUDIO SOURCES")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundSource;
        [SerializeField] private AudioSource _screenSource;

        [Header("AUDIO MAPS")]
        [SerializeField] private List<AudioMapSO> _audioMaps;
        #endregion

        #region PROPERTIES
        public AudioSource MusicSource => _musicSource;
        public AudioSource SoundSource => _soundSource;
        public AudioSource ScreenSource => _screenSource;
        public List<AudioSource> LoopingSources => _loopingSources;
        public float MusicFadeDuration => _musicFadeDuration;
        public List<AudioMapSO> AudioMaps => _audioMaps;
        #endregion

        #region FIELDS PRIVATE
        private readonly List<AudioSource> _loopingSources = new ();
        #endregion
        
        #region METHODS PUBLIC
        public AudioSource GetLoopingSource()
        {
            var freeSource = _loopingSources.Find(source => !source.isPlaying);
            if (freeSource)
            {
                freeSource.volume = _soundSource.volume;
                return freeSource;
            }

            var newSource = gameObject.AddComponent<AudioSource>();
            newSource.volume = _soundSource.volume;
            newSource.playOnAwake = false;
            newSource.loop = true;
            _loopingSources.Add(newSource);

            return newSource;
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            _musicSource.loop = true;
            _soundSource.ignoreListenerPause = false;
            _screenSource.ignoreListenerPause = true;
        }
        #endregion
    }
}
