using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameApplication.Gameplay.Models
{
    [Serializable]
    public class MarsColonyState
    {
        public int Weapons = 50;
        public int Supplies = 50;
        public int People = 50;

        public const int MaxValue = 100;
        public const int MinValue = 0;

        private Dictionary<ResourceType, int> _values;

        public MarsColonyState()
        {
            UpdateDictionary();
        }

        public int GetResource(ResourceType type)
        {
            UpdateDictionary();
            return _values[type];
        }

        public void SetResource(ResourceType type, int value)
        {
            value = Mathf.Clamp(value, MinValue, MaxValue);
            
            switch (type)
            {
                case ResourceType.Weapons:
                    Weapons = value;
                    break;
                case ResourceType.Supplies:
                    Supplies = value;
                    break;
                case ResourceType.People:
                    People = value;
                    break;
            }
            UpdateDictionary();
        }

        public void ModifyResource(ResourceType type, int delta)
        {
            int currentValue = GetResource(type);
            SetResource(type, currentValue + delta);
        }

        public void ApplyCargoDelivery(ShipCargo cargo)
        {
            var resources = cargo.GetAllResources();
            foreach (var resource in resources)
            {
                ModifyResource(resource.Key, resource.Value);
            }
        }

        public void ApplyPeopleConsumption(float weaponsPerPerson, float suppliesPerPerson)
        {
            int totalWeaponsConsumption = Mathf.RoundToInt(People * weaponsPerPerson);
            int totalSuppliesConsumption = Mathf.RoundToInt(People * suppliesPerPerson);

            ModifyResource(ResourceType.Weapons, -totalWeaponsConsumption);
            ModifyResource(ResourceType.Supplies, -totalSuppliesConsumption);

            UnityEngine.Debug.Log($"Потребление людьми ({People} чел): Оружие -{totalWeaponsConsumption}, Припасы -{totalSuppliesConsumption}");
        }

        public bool IsGameOver()
        {
            return Weapons <= MinValue || Weapons >= MaxValue ||
                   Supplies <= MinValue || Supplies >= MaxValue ||
                   People <= MinValue || People >= MaxValue;
        }

        public string GetGameOverReason()
        {
            if (Weapons <= MinValue) return "Martians destroyed the colony due to a lack of weapons!";
            if (Weapons >= MaxValue) return "Militarization led to a revolution!";
            if (Supplies <= MinValue) return "The colonists died of hunger!";
            if (Supplies >= MaxValue) return "Hedonism and excess destroyed the colony!";
            if (People <= MinValue) return "The colony’s population went extinct!";
            if (People >= MaxValue) return "Overpopulation caused a collapse!";
            return "Game over";
        }

        public Dictionary<ResourceType, int> GetAllValues()
        {
            UpdateDictionary();
            return new Dictionary<ResourceType, int>(_values);
        }

        private void UpdateDictionary()
        {
            if (_values == null)
                _values = new Dictionary<ResourceType, int>();

            _values[ResourceType.Weapons] = Weapons;
            _values[ResourceType.Supplies] = Supplies;
            _values[ResourceType.People] = People;
        }
    }
}

