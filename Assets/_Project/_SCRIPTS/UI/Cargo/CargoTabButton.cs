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
        public GameObject selectedVisual;
        public GameObject deselectedVisual;
        
        public ResourceType ResourceType { get; private set; }
        
        private CargoTabController _tabController;
        private bool _isSelected;
        
        public void Initialize(ResourceType resourceType, CargoTabController tabController)
        {
            ResourceType = resourceType;
            _tabController = tabController;
            
            if (button != null)
            {
                button.onClick.AddListener(OnClick);
                button.transition = Selectable.Transition.None;
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
            
            if (selectedVisual != null)
            {
                selectedVisual.SetActive(selected);
            }
            
            if (deselectedVisual != null)
            {
                deselectedVisual.SetActive(!selected);
            }
        }
    }
}
