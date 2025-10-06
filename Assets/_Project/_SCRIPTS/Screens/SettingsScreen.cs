using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameApplication.Screens
{
    public class SettingsScreen : MonoBehaviour
    {
        [SerializeField] private GameObject settingsPanel;
        public static SettingsScreen Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void RestarGame()
        {
            SceneManager.LoadScene(1);
            settingsPanel.SetActive(false);
        }
        
        public void ShowSettings()
        {
            settingsPanel.SetActive(true);
        }
    }
}

