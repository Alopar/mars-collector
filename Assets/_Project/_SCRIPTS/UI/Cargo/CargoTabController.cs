using System.Collections.Generic;
using GameApplication.Gameplay.Models;
using UnityEngine;

namespace GameApplication.UI.Cargo
{
    public class CargoTabController : MonoBehaviour
    {
        [Header("Tab Buttons")]
        public CargoTabButton weaponsTab;
        public CargoTabButton suppliesTab;
        public CargoTabButton peopleTab;
        
        [Header("Panels")]
        public CargoShapePanel weaponsPanel;
        public CargoShapePanel suppliesPanel;
        public CargoShapePanel peoplePanel;
        
        private List<CargoTabButton> _tabs = new List<CargoTabButton>();
        private CargoPlacementController _controller;
        
        private void Start()
        {
            InitializeTabs();
        }
        
        private void InitializeTabs()
        {
            if (weaponsTab != null)
            {
                weaponsTab.Initialize(ResourceType.Weapons, this);
                _tabs.Add(weaponsTab);
            }
            
            if (suppliesTab != null)
            {
                suppliesTab.Initialize(ResourceType.Supplies, this);
                _tabs.Add(suppliesTab);
            }
            
            if (peopleTab != null)
            {
                peopleTab.Initialize(ResourceType.People, this);
                _tabs.Add(peopleTab);
            }
            
            SelectTab(weaponsTab);
        }
        
        public void SetController(CargoPlacementController controller)
        {
            _controller = controller;
        }
        
        public void SelectTab(CargoTabButton selectedTab)
        {
            if (selectedTab == null)
                return;
            
            foreach (var tab in _tabs)
            {
                tab.SetSelected(tab == selectedTab);
            }
            
            ShowPanelForType(selectedTab.ResourceType);
        }
        
        private void ShowPanelForType(ResourceType type)
        {
            if (weaponsPanel != null)
                weaponsPanel.gameObject.SetActive(type == ResourceType.Weapons);
            
            if (suppliesPanel != null)
                suppliesPanel.gameObject.SetActive(type == ResourceType.Supplies);
            
            if (peoplePanel != null)
                peoplePanel.gameObject.SetActive(type == ResourceType.People);
        }
        
        public CargoShapePanel GetActivePanel()
        {
            if (weaponsPanel != null && weaponsPanel.gameObject.activeSelf)
                return weaponsPanel;
            
            if (suppliesPanel != null && suppliesPanel.gameObject.activeSelf)
                return suppliesPanel;
            
            if (peoplePanel != null && peoplePanel.gameObject.activeSelf)
                return peoplePanel;
            
            return null;
        }
    }
}
