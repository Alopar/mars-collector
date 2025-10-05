using UnityEngine;

namespace GameApplication.Gameplay.Models.Missions
{
    [CreateAssetMenu(fileName = "Missions Chain", menuName = "Mars Collector/Missions Chain")]
    public class MissionsChainConfig : ScriptableObject
    {
        public MissionConfig[] Missions;
    }
}