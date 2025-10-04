using System.Collections.Generic;
using UnityEngine;

namespace GameApplication.Gameplay.Models.Cargo
{
    [CreateAssetMenu(fileName = "CargoShapeDatabase", menuName = "Mars Collector/Cargo Shape Database")]
    public class CargoShapeDatabase : ScriptableObject
    {
        [Header("Weapon Shapes")]
        public List<CargoShapeData> weaponShapes = new List<CargoShapeData>();
        
        [Header("Supply Shapes")]
        public List<CargoShapeData> supplyShapes = new List<CargoShapeData>();
        
        [Header("People Shapes")]
        public List<CargoShapeData> peopleShapes = new List<CargoShapeData>();
        
        public List<CargoShapeData> GetShapesForType(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Weapons:
                    return weaponShapes;
                case ResourceType.Supplies:
                    return supplyShapes;
                case ResourceType.People:
                    return peopleShapes;
                default:
                    return new List<CargoShapeData>();
            }
        }
        
        public List<CargoShapeData> GetAllShapes()
        {
            var allShapes = new List<CargoShapeData>();
            allShapes.AddRange(weaponShapes);
            allShapes.AddRange(supplyShapes);
            allShapes.AddRange(peopleShapes);
            return allShapes;
        }
    }
}

