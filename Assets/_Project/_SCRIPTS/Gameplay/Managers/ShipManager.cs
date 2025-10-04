using System;
using GameApplication.Gameplay.Models;
using UnityEngine;

namespace GameApplication.Gameplay.Managers
{
    public class ShipManager : MonoBehaviour
    {
        public static ShipManager Instance { get; private set; }

        public ShipCargo CurrentCargo { get; private set; }

        public event Action<ResourceType, int> OnResourceAdded;
        public event Action<ResourceType, int> OnResourceRemoved;
        public event Action OnCargoCleared;
        public event Action<ShipCargo> OnShipLaunched;
        public event Action<int, int> OnCargoChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Initialize(int shipCapacity)
        {
            CurrentCargo = new ShipCargo(shipCapacity);
        }

        public bool LoadResource(ResourceType type, int amount = 1)
        {
            if (CurrentCargo.AddResource(type, amount))
            {
                OnResourceAdded?.Invoke(type, amount);
                OnCargoChanged?.Invoke(CurrentCargo.GetTotalResources(), CurrentCargo.MaxCapacity);
                return true;
            }
            return false;
        }

        public bool UnloadResource(ResourceType type, int amount = 1)
        {
            if (CurrentCargo.RemoveResource(type, amount))
            {
                OnResourceRemoved?.Invoke(type, amount);
                OnCargoChanged?.Invoke(CurrentCargo.GetTotalResources(), CurrentCargo.MaxCapacity);
                return true;
            }
            return false;
        }

        public void LaunchShip()
        {
            if (CurrentCargo.GetTotalResources() == 0)
            {
                Debug.LogWarning("Cannot launch empty ship!");
                return;
            }

            OnShipLaunched?.Invoke(CurrentCargo);
        }

        public void ClearCargo()
        {
            CurrentCargo.Clear();
            OnCargoCleared?.Invoke();
            OnCargoChanged?.Invoke(0, CurrentCargo.MaxCapacity);
        }

        public int GetResourceAmount(ResourceType type)
        {
            return CurrentCargo.GetResource(type);
        }

        public bool CanAddResource(int amount = 1)
        {
            return CurrentCargo.CanAddResource(amount);
        }

        public int GetAvailableSpace()
        {
            return CurrentCargo.GetAvailableSpace();
        }
    }
}
