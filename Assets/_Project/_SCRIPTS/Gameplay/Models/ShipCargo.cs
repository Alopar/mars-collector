using System;
using System.Collections.Generic;

namespace GameApplication.Gameplay.Models
{
    [Serializable]
    public class ShipCargo
    {
        private Dictionary<ResourceType, int> _resources;
        
        public int MaxCapacity = 10;

        public ShipCargo(int maxCapacity = 10)
        {
            MaxCapacity = maxCapacity;
            _resources = new Dictionary<ResourceType, int>
            {
                { ResourceType.Weapons, 0 },
                { ResourceType.Supplies, 0 },
                { ResourceType.People, 0 }
            };
        }

        public int GetResource(ResourceType type)
        {
            return _resources[type];
        }

        public int GetTotalResources()
        {
            int total = 0;
            foreach (var amount in _resources.Values)
            {
                total += amount;
            }
            return total;
        }

        public int GetAvailableSpace()
        {
            return MaxCapacity - GetTotalResources();
        }

        public bool CanAddResource(int amount = 1)
        {
            return GetTotalResources() + amount <= MaxCapacity;
        }

        public bool AddResource(ResourceType type, int amount = 1)
        {
            if (!CanAddResource(amount))
                return false;

            _resources[type] += amount;
            return true;
        }

        public bool RemoveResource(ResourceType type, int amount = 1)
        {
            if (_resources[type] < amount)
                return false;

            _resources[type] -= amount;
            return true;
        }

        public void Clear()
        {
            _resources[ResourceType.Weapons] = 0;
            _resources[ResourceType.Supplies] = 0;
            _resources[ResourceType.People] = 0;
        }

        public Dictionary<ResourceType, int> GetAllResources()
        {
            return new Dictionary<ResourceType, int>(_resources);
        }
    }
}
