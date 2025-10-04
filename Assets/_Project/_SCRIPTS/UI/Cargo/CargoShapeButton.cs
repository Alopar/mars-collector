using GameApplication.Gameplay.Models;
using GameApplication.Gameplay.Models.Cargo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameApplication.UI.Cargo
{
    public class CargoShapeButton : MonoBehaviour
    {
        [Header("References")]
        public Button button;
        public Image icon;
        public TextMeshProUGUI amountText;
        public GameObject selectionFrame;
        
        public CargoShapeData ShapeData { get; private set; }
        
        private CargoShapePanel _panel;
        
        public void Initialize(CargoShapeData shapeData, CargoShapePanel panel)
        {
            ShapeData = shapeData;
            _panel = panel;
            
            if (icon != null && shapeData.icon != null)
            {
                icon.sprite = shapeData.icon;
                icon.color = Color.white;
            }
            else if (icon != null)
            {
                icon.color = GetColorForType(shapeData.resourceType);
            }
            
            if (amountText != null)
            {
                amountText.text = $"x{shapeData.resourceAmount}";
            }
            
            if (button != null)
            {
                button.onClick.AddListener(OnClick);
            }
            
            SetSelected(false);
        }
        
        private Color GetColorForType(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Weapons: return new Color(1f, 0.3f, 0.3f);
                case ResourceType.Supplies: return new Color(0.3f, 1f, 0.3f);
                case ResourceType.People: return new Color(0.3f, 0.3f, 1f);
                default: return Color.white;
            }
        }
        
        private void OnClick()
        {
            if (_panel != null)
            {
                _panel.OnShapeButtonClicked(this);
            }
        }
        
        public void SetSelected(bool selected)
        {
            if (selectionFrame != null)
            {
                selectionFrame.SetActive(selected);
            }
        }
    }
}

