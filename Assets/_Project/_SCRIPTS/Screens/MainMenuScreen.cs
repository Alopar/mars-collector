using GameApplication.Gameplay.Managers;
using UnityEngine;

namespace GameApplication.Screens
{
    public class MainMenuScreen : MonoBehaviour
    {

        public void PlayGame()
        {
            GameManager.Instance.StartGame();
        }
        
        public void Settings()
        {
            SettingsScreen.Instance.ShowSettings();
        }
    }
}

