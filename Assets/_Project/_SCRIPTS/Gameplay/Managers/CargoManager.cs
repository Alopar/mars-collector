using System;
using System.Collections.Generic;
using GameApplication.Gameplay.Models;
using GameApplication.Gameplay.Models.Cargo;
using UnityEngine;

namespace GameApplication.Gameplay.Managers
{
    public class CargoManager : MonoBehaviour
    {
        public static CargoManager Instance { get; private set; }
        
        public CargoGrid Grid { get; private set; }
        public CargoShapeDatabase Database { get; private set; }
        
        public event Action<ResourceType, int> OnResourcesChanged;
        public event Action<PlacedShapeData> OnShapePlaced;
        public event Action<PlacedShapeData> OnShapeRemoved;
        public event Action OnCargoCleared;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
        
        public void Initialize(int width, int height, CargoShapeDatabase database)
        {
            Grid = new CargoGrid(width, height);
            Database = database;
            
            Debug.Log($"CargoManager initialized: {width}x{height} grid");
        }
        
        public bool PlaceShape(CargoShapeData shape, int x, int y)
        {
            if (Grid == null)
            {
                Debug.LogError("CargoGrid not initialized!");
                return false;
            }
            
            var placed = Grid.PlaceShape(shape, x, y);
            
            if (placed != null)
            {
                OnShapePlaced?.Invoke(placed);
                OnResourcesChanged?.Invoke(shape.resourceType, shape.resourceAmount);
                Debug.Log($"Placed {shape.shapeName} at ({x}, {y}), +{shape.resourceAmount} {shape.resourceType}");
                return true;
            }
            
            return false;
        }
        
        public bool RemoveShapeAt(int x, int y)
        {
            if (Grid == null)
            {
                Debug.LogError("CargoGrid not initialized!");
                return false;
            }
            
            var removed = Grid.RemoveShapeAt(x, y);
            
            if (removed != null)
            {
                OnShapeRemoved?.Invoke(removed);
                OnResourcesChanged?.Invoke(removed.shape.resourceType, -removed.shape.resourceAmount);
                Debug.Log($"Removed {removed.shape.shapeName} at ({x}, {y}), -{removed.shape.resourceAmount} {removed.shape.resourceType}");
                return true;
            }
            
            return false;
        }
        
        public Dictionary<ResourceType, int> GetLoadedResources()
        {
            if (Grid == null)
            {
                return new Dictionary<ResourceType, int>
                {
                    { ResourceType.Weapons, 0 },
                    { ResourceType.Supplies, 0 },
                    { ResourceType.People, 0 }
                };
            }
            
            return Grid.CalculateLoadedResources();
        }
        
        public void ClearCargo()
        {
            if (Grid != null)
            {
                Grid.Clear();
                OnCargoCleared?.Invoke();
                
                OnResourcesChanged?.Invoke(ResourceType.Weapons, 0);
                OnResourcesChanged?.Invoke(ResourceType.Supplies, 0);
                OnResourcesChanged?.Invoke(ResourceType.People, 0);
                
                Debug.Log("Cargo cleared");
            }
        }
        
        public ShipCargo ConvertToShipCargo()
        {
            var resources = GetLoadedResources();
            var cargo = new ShipCargo(Grid.Width * Grid.Height);
            
            foreach (var resource in resources)
            {
                for (int i = 0; i < resource.Value; i++)
                {
                    cargo.AddResource(resource.Key, 1);
                }
            }
            
            Debug.Log($"Converted to ShipCargo: W:{resources[ResourceType.Weapons]} S:{resources[ResourceType.Supplies]} P:{resources[ResourceType.People]}");
            return cargo;
        }
        
        public bool HasAnyShapes()
        {
            return Grid != null && Grid.GetPlacedShapes().Count > 0;
        }
    }
}

