using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Managers
{
    public class SoundManager : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundSource;

        [Space(10)]
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundSlider;

        [Space(10)]
        [SerializeField] private AudioClip _sampleSound;
        [SerializeField, Range(0, 1)] private float _sampleDelay = 0.25f;
        #endregion

        #region FIELDS PRIVATE
        private static SoundManager _instance;
        private float _sampleTimer;
        #endregion

        #region PROPERTIES
        public static SoundManager Instance => _instance;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            _instance = this;
        }

        private void OnEnable()
        {
            _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            _soundSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
        }

        private void OnDisable()
        {
            _musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            _soundSlider.onValueChanged.RemoveListener(OnSoundVolumeChanged);
        }

        private void Start()
        {
            _sampleTimer = _sampleDelay + Time.unscaledTime;
            _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.25f);
            _soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        }
        #endregion

        #region METHODS PRIVATE
        private void OnMusicVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            _musicSource.volume = value;
        }

        private void OnSoundVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("SoundVolume", value);
            _soundSource.volume = value;

            if (_sampleTimer > Time.unscaledTime) return;

            _sampleTimer = _sampleDelay + Time.unscaledTime;
            PlaySound(_sampleSound);
        }
        #endregion

        #region METHODS PUBLIC
        public void PlaySound(AudioClip clip)
        {
            _soundSource.PlayOneShot(clip);
        }
        #endregion
    }
}
