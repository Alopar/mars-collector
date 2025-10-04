using UnityEngine;
using UnityEngine.UI;

namespace GameApplication.Gameplay.Managers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundSource;

        [Space(10)]
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundSlider;

        [Space(10)]
        [SerializeField] private AudioClip _sampleSound;
        [SerializeField, Range(0, 1)] private float _sampleDelay = 0.25f;

        private float _sampleTimer;

        public static SoundManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            // _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            // _soundSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
        }

        private void OnDisable()
        {
            // _musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            // _soundSlider.onValueChanged.RemoveListener(OnSoundVolumeChanged);
        }

        private void Start()
        {
            // _sampleTimer = _sampleDelay + Time.unscaledTime;
            // _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.25f);
            // _soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        }

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

        public void PlaySound(AudioClip clip)
        {
            _soundSource.PlayOneShot(clip);
        }

    }
}
