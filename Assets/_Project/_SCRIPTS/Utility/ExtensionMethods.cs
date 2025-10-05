using GameApplication.Gameplay.Managers;
using GameApplication.Gameplay.Models;
using UnityEngine;
using UnityEngine.UI;

namespace GameApplication.Utility
{
    public static class ExtensionMethods
    {
        public static void SetAlpha(this Image image, float alpha) =>
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

        public static float GetWeaponsPerPerson(this GameConfig gameConfig)
        {
            if (gameConfig.WeaponsPerPersonForMissionsComplete == null || gameConfig.WeaponsPerPersonForMissionsComplete.Length == 0)
                return 0.5f;
            
            int index = 0;
            if (MissionsManager.Instance != null)
            {
                index = Mathf.Clamp(MissionsManager.Instance.CurrentMissionIndex, 0, gameConfig.WeaponsPerPersonForMissionsComplete.Length - 1);
            }
            
            return gameConfig.WeaponsPerPersonForMissionsComplete[index];
        }

        public static float GetSuppliesPerPerson(this GameConfig gameConfig)
        {
            if (gameConfig.SuppliesPerPersonForMissionsComplete == null || gameConfig.SuppliesPerPersonForMissionsComplete.Length == 0)
                return 1f;
            
            int index = 0;
            if (MissionsManager.Instance != null)
            {
                index = Mathf.Clamp(MissionsManager.Instance.CurrentMissionIndex, 0, gameConfig.SuppliesPerPersonForMissionsComplete.Length - 1);
            }
            
            return gameConfig.SuppliesPerPersonForMissionsComplete[index];
        }
    }
}