using UnityEngine;

namespace GameApplication.Gameplay.Models
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Mars Collector/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Ship Settings")]
        public int ShipCapacity = 10;

        [Header("Mars Starting Values")]
        public int StartingWeapons = 50;
        public int StartingSupplies = 50;
        public int StartingPeople = 50;

        [Header("People Consumption")]
        [Tooltip("Сколько оружия потребляет 1 человек за ход")]
        public float WeaponsPerPerson = 0.1f;
        
        [Tooltip("Сколько припасов потребляет 1 человек за ход")]
        public float SuppliesPerPerson = 0.2f;

        [Header("Win Condition")]
        public int TurnsToWin = 20;

        [Header("Events")]
        public TextAsset EventsJson;
    }
}
