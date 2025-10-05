using GameApplication.Gameplay.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameApplication.UI.Cargo
{
    public class CargoTabButton : MonoBehaviour
    {
        [Header("References")]
        public Button button;
        public Image background;
        public TextMeshProUGUI label;
        public Image icon;
        
        [Header("Colors")]
        public Color selectedColor = new Color(0.2f, 0.2f, 0.2f);
        public Color deselectedColor = new Color(0.5f, 0.5f, 0.5f);
        
        public ResourceType ResourceType { get; private set; }
        
        private CargoTabController _tabController;
        private bool _isSelected;
        
        public void Initialize(ResourceType resourceType, CargoTabController tabController)
        {
            ResourceType = resourceType;
            _tabController = tabController;
            
            if (label != null)
            {
                label.text = GetLabelForType(resourceType);
            }
            
            if (icon != null)
            {
                icon.color = GetColorForType(resourceType);
            }
            
            if (button != null)
            {
                button.onClick.AddListener(OnClick);
            }
            
            SetSelected(false);
        }
        
        private void OnClick()
        {
            if (_tabController != null)
            {
                _tabController.SelectTab(this);
            }
        }
        
        public void SetSelected(bool selected)
        {
            _isSelected = selected;
            
            if (background != null)
            {
                background.color = selected ? selectedColor : deselectedColor;
            }
        }
        
        private string GetLabelForType(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Weapons: return "Оружие";
                case ResourceType.Supplies: return "Припасы";
                case ResourceType.People: return "Люди";
                default: return "???";
            }
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
    }
}
