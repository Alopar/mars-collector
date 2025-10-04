using System;
using System.Collections.Generic;

namespace GameApplication.Gameplay.Models
{
    [Serializable]
    public class TurnEvent
    {
        public string id;
        public string title;
        public string description;
        public ResourceChanges resourceChanges;

        public Dictionary<ResourceType, int> GetResourceChanges()
        {
            var changes = new Dictionary<ResourceType, int>();
            
            if (resourceChanges == null)
                return changes;

            changes[ResourceType.Weapons] = resourceChanges.weapons;
            changes[ResourceType.Supplies] = resourceChanges.supplies;
            changes[ResourceType.People] = resourceChanges.people;

            return changes;
        }
    }

    [Serializable]
    public class ResourceChanges
    {
        public int weapons;
        public int supplies;
        public int people;
    }

    [Serializable]
    public class EventSequence
    {
        public List<TurnEvent> events;
    }
}
