using UnityEngine;

namespace GameApplication.Gameplay.Models.Missions
{
    [CreateAssetMenu(fileName = "Mission", menuName = "Mars Collector/Mission")]
    public class MissionConfig : ScriptableObject
    {
        public string MissionName = "";
        public string MissionDescription = "";
        public string MissionResult = "";
        public int MinPeopleNeededToStart;

        public MissionResourceRequirement[] ResourceRequirements;
        //public SomeOtherRequirements[] SomeOtherRequirements; 
        // тут можно расширить другими типами требований

        public bool CheckRequirements()
        {
            foreach (var requirement in ResourceRequirements)
            {
                if (!requirement.IsCompleted())
                    return false;
            }
            return true;
        }
    }
}