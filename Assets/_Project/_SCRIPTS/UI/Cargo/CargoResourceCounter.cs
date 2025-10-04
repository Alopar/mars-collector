using System.Collections.Generic;
using GameApplication.Gameplay.Managers;
using GameApplication.Gameplay.Models;
using TMPro;
using UnityEngine;

namespace GameApplication.UI.Cargo
{
    public class CargoResourceCounter : MonoBehaviour
    {
        [Header("Text References")]
        public TextMeshProUGUI weaponsText;
        public TextMeshProUGUI suppliesText;
        public TextMeshProUGUI peopleText;
        
        private void Start()
        {
            if (CargoManager.Instance != null)
            {
                CargoManager.Instance.OnResourcesChanged += OnResourcesChanged;
                CargoManager.Instance.OnCargoCleared += OnCargoCleared;
            }
            
            UpdateDisplay();
        }
        
        private void OnResourcesChanged(ResourceType type, int delta)
        {
            UpdateDisplay();
        }
        
        private void OnCargoCleared()
        {
            UpdateDisplay();
        }
        
        private void UpdateDisplay()
        {
            if (CargoManager.Instance == null)
                return;
            
            var resources = CargoManager.Instance.GetLoadedResources();
            
            if (weaponsText != null)
            {
                weaponsText.text = $"Оружие: {resources[ResourceType.Weapons]}";
            }
            
            if (suppliesText != null)
            {
                suppliesText.text = $"Припасы: {resources[ResourceType.Supplies]}";
            }
            
            if (peopleText != null)
            {
                peopleText.text = $"Люди: {resources[ResourceType.People]}";
            }
        }
        
        private void OnDestroy()
        {
            if (CargoManager.Instance != null)
            {
                CargoManager.Instance.OnResourcesChanged -= OnResourcesChanged;
                CargoManager.Instance.OnCargoCleared -= OnCargoCleared;
            }
        }
    }
}

