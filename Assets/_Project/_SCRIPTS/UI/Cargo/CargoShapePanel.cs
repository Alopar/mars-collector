using System.Collections.Generic;
using GameApplication.Gameplay.Models;
using GameApplication.Gameplay.Models.Cargo;
using UnityEngine;

namespace GameApplication.UI.Cargo
{
    public class CargoShapePanel : MonoBehaviour
    {
        [Header("Settings")]
        public ResourceType resourceType;
        
        [Header("References")]
        public GameObject buttonPrefab;
        public Transform buttonsContainer;
        
        private List<CargoShapeButton> _buttons = new List<CargoShapeButton>();
        private CargoPlacementController _controller;
        
        public void Initialize(List<CargoShapeData> shapes, CargoPlacementController controller)
        {
            _controller = controller;
            
            foreach (var shape in shapes)
            {
                GameObject buttonObj = Instantiate(buttonPrefab, buttonsContainer);
                var button = buttonObj.GetComponent<CargoShapeButton>();
                
                if (button != null)
                {
                    button.Initialize(shape, this);
                    _buttons.Add(button);
                }
            }
        }
        
        public void OnShapeButtonClicked(CargoShapeButton button)
        {
            if (_controller != null && button.ShapeData != null)
            {
                _controller.SelectShape(button.ShapeData);
            }
            
            button.SetSelected(true);
        }
        
        public void DeselectAll()
        {
            foreach (var button in _buttons)
            {
                button.SetSelected(false);
            }
        }
    }
}

