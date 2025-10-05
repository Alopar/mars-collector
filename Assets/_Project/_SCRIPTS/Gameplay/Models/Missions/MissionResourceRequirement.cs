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

            int totalResourcesAmount = CargoManager.Instance.Grid.GetTotalLoadedAmount();

            return totalResourcesAmount == currentAmount && currentAmount >= MinAmount;
        }
    }
}