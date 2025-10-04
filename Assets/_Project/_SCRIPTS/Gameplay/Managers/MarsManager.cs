using System;
using System.Collections.Generic;
using GameApplication.Gameplay.Models;
using UnityEngine;

namespace GameApplication.Gameplay.Managers
{
    public class MarsManager : MonoBehaviour
    {
        public static MarsManager Instance { get; private set; }

        public MarsColonyState ColonyState { get; private set; }
        
        private Dictionary<ResourceType, int> _previewChanges = new Dictionary<ResourceType, int>();

        public event Action<MarsColonyState> OnColonyStateChanged;
        public event Action<Dictionary<ResourceType, int>> OnPreviewUpdated;
        public event Action<string> OnGameOver;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Initialize(int weapons, int supplies, int people)
        {
            ColonyState = new MarsColonyState
            {
                Weapons = weapons,
                Supplies = supplies,
                People = people
            };
            
            OnColonyStateChanged?.Invoke(ColonyState);
            ClearPreview();
        }

        public void SetPreview(Dictionary<ResourceType, int> changes)
        {
            _previewChanges = new Dictionary<ResourceType, int>(changes);
            OnPreviewUpdated?.Invoke(_previewChanges);
        }

        public void ClearPreview()
        {
            _previewChanges.Clear();
            OnPreviewUpdated?.Invoke(_previewChanges);
        }

        public Dictionary<ResourceType, int> GetPreview()
        {
            return new Dictionary<ResourceType, int>(_previewChanges);
        }

        public void ApplyChanges(Dictionary<ResourceType, int> changes)
        {
            foreach (var change in changes)
            {
                ColonyState.ModifyResource(change.Key, change.Value);
            }

            OnColonyStateChanged?.Invoke(ColonyState);

            if (ColonyState.IsGameOver())
            {
                OnGameOver?.Invoke(ColonyState.GetGameOverReason());
            }
        }

        public void ApplyCargoDelivery(ShipCargo cargo)
        {
            ColonyState.ApplyCargoDelivery(cargo);
            OnColonyStateChanged?.Invoke(ColonyState);
        }

        public void ApplyPeopleConsumption(float weaponsPerPerson, float suppliesPerPerson)
        {
            ColonyState.ApplyPeopleConsumption(weaponsPerPerson, suppliesPerPerson);
            OnColonyStateChanged?.Invoke(ColonyState);

            if (ColonyState.IsGameOver())
            {
                OnGameOver?.Invoke(ColonyState.GetGameOverReason());
            }
        }

        public int GetResourceValue(ResourceType type)
        {
            return ColonyState.GetResource(type);
        }

        public void ResetColony(int weapons, int supplies, int people)
        {
            Initialize(weapons, supplies, people);
        }
    }
}
