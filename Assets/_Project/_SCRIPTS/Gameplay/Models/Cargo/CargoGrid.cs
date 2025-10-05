using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameApplication.Gameplay.Models.Cargo
{
    [Serializable]
    public struct CargoCell
    {
        public bool isOccupied;
        public int shapeInstanceId;
    }
    
    public class CargoGrid
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        
        private CargoCell[,] _cells;
        private List<PlacedShapeData> _placedShapes;
        private int _nextInstanceId = 1;
        
        public CargoGrid(int width, int height)
        {
            Width = width;
            Height = height;
            _cells = new CargoCell[height, width];
            _placedShapes = new List<PlacedShapeData>();
        }
        
        public bool CanPlaceShape(CargoShapeData shape, int x, int y)
        {
            if (shape == null)
                return false;
            
            var positions = shape.GetOccupiedPositions();
            
            foreach (var offset in positions)
            {
                int cellX = x + offset.x;
                int cellY = y + offset.y;
                
                if (cellX < 0 || cellX >= Width || cellY < 0 || cellY >= Height)
                    return false;
                
                if (_cells[cellY, cellX].isOccupied)
                    return false;
            }
            
            return true;
        }
        
        public PlacedShapeData PlaceShape(CargoShapeData shape, int x, int y)
        {
            if (!CanPlaceShape(shape, x, y))
                return null;
            
            int instanceId = _nextInstanceId++;
            var positions = shape.GetOccupiedPositions();
            
            foreach (var offset in positions)
            {
                int cellX = x + offset.x;
                int cellY = y + offset.y;
                
                _cells[cellY, cellX].isOccupied = true;
                _cells[cellY, cellX].shapeInstanceId = instanceId;
            }
            
            var placedShape = new PlacedShapeData(shape, new Vector2Int(x, y), instanceId);
            _placedShapes.Add(placedShape);
            
            return placedShape;
        }
        
        public PlacedShapeData RemoveShapeAt(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return null;
            
            if (!_cells[y, x].isOccupied)
                return null;
            
            int instanceId = _cells[y, x].shapeInstanceId;
            
            PlacedShapeData shapeToRemove = _placedShapes.Find(s => s.instanceId == instanceId);
            if (shapeToRemove == null)
                return null;
            
            var positions = shapeToRemove.shape.GetOccupiedPositions();
            foreach (var offset in positions)
            {
                int cellX = shapeToRemove.position.x + offset.x;
                int cellY = shapeToRemove.position.y + offset.y;
                
                if (cellX >= 0 && cellX < Width && cellY >= 0 && cellY < Height)
                {
                    _cells[cellY, cellX].isOccupied = false;
                    _cells[cellY, cellX].shapeInstanceId = 0;
                }
            }
            
            _placedShapes.Remove(shapeToRemove);
            return shapeToRemove;
        }
        
        public Dictionary<ResourceType, int> CalculateLoadedResources()
        {
            var resources = new Dictionary<ResourceType, int>
            {
                { ResourceType.Weapons, 0 },
                { ResourceType.Supplies, 0 },
                { ResourceType.People, 0 }
            };
            
            foreach (var placedShape in _placedShapes)
            {
                if (placedShape.shape != null)
                {
                    resources[placedShape.shape.resourceType] += placedShape.shape.resourceAmount;
                }
            }
            
            return resources;
        }
        
        public void Clear()
        {
            _cells = new CargoCell[Height, Width];
            _placedShapes.Clear();
            _nextInstanceId = 1;
        }
        
        public CargoCell GetCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return new CargoCell();
            
            return _cells[y, x];
        }
        
        public List<PlacedShapeData> GetPlacedShapes()
        {
            return new List<PlacedShapeData>(_placedShapes);
        }
        
        public PlacedShapeData GetShapeAt(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return null;
            
            if (!_cells[y, x].isOccupied)
                return null;
            
            int instanceId = _cells[y, x].shapeInstanceId;
            return _placedShapes.Find(s => s.instanceId == instanceId);
        }
        
        public int GetTotalLoadedAmount()
        {
            var resources = CalculateLoadedResources();
            int total = 0;
            foreach (var amount in resources.Values)
            {
                total += amount;
            }
            return total;
        }
    }
}

