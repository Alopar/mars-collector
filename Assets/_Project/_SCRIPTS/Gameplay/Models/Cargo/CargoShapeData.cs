using System.Collections.Generic;
using UnityEngine;

namespace GameApplication.Gameplay.Models.Cargo
{
    [CreateAssetMenu(fileName = "CargoShape", menuName = "Mars Collector/Cargo Shape")]
    public class CargoShapeData : ScriptableObject
    {
        [Header("Basic Info")]
        public string shapeName = "New Shape";
        public ResourceType resourceType = ResourceType.Weapons;
        public Sprite icon;
        
        [Header("Shape Definition")]
        [Tooltip("Ширина фигуры")]
        public int shapeWidth = 2;
        
        [Tooltip("Высота фигуры")]
        public int shapeHeight = 2;
        
        [Tooltip("Плоский массив: 1 = занято, 0 = пусто. Размер должен быть = width * height")]
        public int[] shapeFlat = new int[] { 1, 1, 1, 1 };
        
        [Header("Gameplay")]
        [Tooltip("Сколько единиц ресурса дает эта фигура")]
        public int resourceAmount = 1;
        
        [Header("Visual")]
        public Color previewColor = new Color(0, 1, 0, 0.3f);
        
        public int[,] GetShape()
        {
            if (shapeFlat == null || shapeFlat.Length != shapeWidth * shapeHeight)
            {
                Debug.LogError($"Invalid shape data in {shapeName}: expected {shapeWidth * shapeHeight} elements, got {shapeFlat?.Length ?? 0}");
                return new int[shapeHeight, shapeWidth];
            }
            
            int[,] shape = new int[shapeHeight, shapeWidth];
            for (int y = 0; y < shapeHeight; y++)
            {
                for (int x = 0; x < shapeWidth; x++)
                {
                    shape[y, x] = shapeFlat[y * shapeWidth + x];
                }
            }
            return shape;
        }
        
        public bool IsShapeCellOccupied(int x, int y)
        {
            if (x < 0 || x >= shapeWidth || y < 0 || y >= shapeHeight)
                return false;
            
            if (shapeFlat == null || shapeFlat.Length != shapeWidth * shapeHeight)
                return false;
            
            return shapeFlat[y * shapeWidth + x] == 1;
        }
        
        public List<Vector2Int> GetOccupiedPositions()
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            
            if (shapeFlat == null)
                return positions;
            
            for (int y = 0; y < shapeHeight; y++)
            {
                for (int x = 0; x < shapeWidth; x++)
                {
                    int index = y * shapeWidth + x;
                    if (index < shapeFlat.Length && shapeFlat[index] == 1)
                    {
                        positions.Add(new Vector2Int(x, y));
                    }
                }
            }
            return positions;
        }
        
        public int GetOccupiedCellCount()
        {
            if (shapeFlat == null)
                return 0;
            
            int count = 0;
            foreach (var cell in shapeFlat)
            {
                if (cell == 1)
                    count++;
            }
            return count;
        }
        
        public float GetEfficiency()
        {
            int occupiedCells = GetOccupiedCellCount();
            if (occupiedCells == 0)
                return 0f;
            
            return (float)resourceAmount / occupiedCells;
        }
    }
}

