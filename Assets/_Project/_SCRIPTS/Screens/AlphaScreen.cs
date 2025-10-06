using UnityEngine;

namespace GameApplication.Screens
{
    public class AlphaScreen : MonoBehaviour
    {
        public void ShowSettings()
        {
            SettingsScreen.Instance.ShowSettings();
        }
    }
}

