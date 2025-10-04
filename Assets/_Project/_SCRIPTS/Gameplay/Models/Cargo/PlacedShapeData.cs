using System;
using UnityEngine;

namespace GameApplication.Gameplay.Models.Cargo
{
    [Serializable]
    public class PlacedShapeData
    {
        public CargoShapeData shape;
        public Vector2Int position;
        public int instanceId;
        
        public PlacedShapeData(CargoShapeData shape, Vector2Int position, int instanceId)
        {
            this.shape = shape;
            this.position = position;
            this.instanceId = instanceId;
        }
    }
}

