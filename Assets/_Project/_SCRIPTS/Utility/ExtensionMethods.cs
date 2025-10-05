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

        public static float GetWeaponsPerPerson(this GameConfig gameConfig) =>
            gameConfig.WeaponsPerPersonForMissionsComplete[Mathf.Max(MissionsManager.Instance.CurrentMissionIndex, 0)];

        public static float GetSuppliesPerPerson(this GameConfig gameConfig) =>
            gameConfig.SuppliesPerPersonForMissionsComplete[Mathf.Max(MissionsManager.Instance.CurrentMissionIndex, 0)];
    }
}