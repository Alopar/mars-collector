using GameApplication.Gameplay.Managers;
using System;

namespace GameApplication.Gameplay.Models.Missions
{
    [Serializable]
    public class MissionResourceRequirement
    {
        public ResourceType ResourceType;
        public int MinAmount;

        public virtual bool IsCompleted()
        {
            CargoManager.Instance.Grid.CalculateLoadedResources()
                .TryGetValue(ResourceType, out int currentAmount);

            return currentAmount >= MinAmount;
        }
    }
}