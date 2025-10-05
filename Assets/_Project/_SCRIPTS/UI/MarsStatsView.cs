using UnityEngine;
using TMPro;
using System.Collections.Generic;
using GameApplication.Gameplay.Managers;
using GameApplication.Gameplay.Models;

namespace GameApplication.UI
{
    public class MarsStatsView : MonoBehaviour
    {
        [Header("Weapons Row")]
        public TextMeshProUGUI weaponsCurrent;
        public TextMeshProUGUI weaponsExpenses;
        public TextMeshProUGUI weaponsLoaded;
        public TextMeshProUGUI weaponsTotal;
        
        [Header("Supplies Row")]
        public TextMeshProUGUI suppliesCurrent;
        public TextMeshProUGUI suppliesExpenses;
        public TextMeshProUGUI suppliesLoaded;
        public TextMeshProUGUI suppliesTotal;
        
        [Header("People Row")]
        public TextMeshProUGUI peopleCurrent;
        public TextMeshProUGUI peopleExpenses;
        public TextMeshProUGUI peopleLoaded;
        public TextMeshProUGUI peopleTotal;
        
        private MarsColonyState _currentState;
        private Dictionary<ResourceType, int> _eventChanges = new Dictionary<ResourceType, int>();
        private Dictionary<ResourceType, int> _cargoLoaded = new Dictionary<ResourceType, int>();

        private void Start()
        {
            if (MarsManager.Instance != null)
            {
                MarsManager.Instance.OnColonyStateChanged += OnColonyStateChanged;
                MarsManager.Instance.OnPreviewUpdated += OnPreviewUpdated;
            }
            
            if (CargoManager.Instance != null)
            {
                CargoManager.Instance.OnResourcesChanged += OnCargoChanged;
                CargoManager.Instance.OnCargoCleared += OnCargoCleared;
            }
            
            UpdateDisplay();
        }

        private void OnColonyStateChanged(MarsColonyState state)
        {
            _currentState = state;
            UpdateDisplay();
        }

        private void OnPreviewUpdated(Dictionary<ResourceType, int> preview)
        {
            _eventChanges = new Dictionary<ResourceType, int>(preview);
            UpdateDisplay();
        }
        
        private void OnCargoChanged(ResourceType type, int delta)
        {
            UpdateCargoData();
            UpdateDisplay();
        }
        
        private void OnCargoCleared()
        {
            UpdateCargoData();
            UpdateDisplay();
        }
        
        private void UpdateCargoData()
        {
            if (CargoManager.Instance != null)
            {
                _cargoLoaded = CargoManager.Instance.GetLoadedResources();
            }
            else
            {
                _cargoLoaded = new Dictionary<ResourceType, int>
                {
                    { ResourceType.Weapons, 0 },
                    { ResourceType.Supplies, 0 },
                    { ResourceType.People, 0 }
                };
            }
        }

        private void UpdateDisplay()
        {
            if (_currentState == null)
            {
                if (MarsManager.Instance != null)
                {
                    _currentState = MarsManager.Instance.ColonyState;
                }
                else
                {
                    return;
                }
            }
            
            if (_cargoLoaded == null || _cargoLoaded.Count == 0)
            {
                UpdateCargoData();
            }
            
            UpdateRow(
                ResourceType.Weapons,
                _currentState.Weapons,
                weaponsCurrent, weaponsExpenses, weaponsLoaded, weaponsTotal
            );
            
            UpdateRow(
                ResourceType.Supplies,
                _currentState.Supplies,
                suppliesCurrent, suppliesExpenses, suppliesLoaded, suppliesTotal
            );
            
            UpdateRow(
                ResourceType.People,
                _currentState.People,
                peopleCurrent, peopleExpenses, peopleLoaded, peopleTotal
            );
        }

        private void UpdateRow(
            ResourceType type,
            int current,
            TextMeshProUGUI currentText,
            TextMeshProUGUI expensesText,
            TextMeshProUGUI loadedText,
            TextMeshProUGUI totalText)
        {
            int expenses = _eventChanges.ContainsKey(type) ? _eventChanges[type] : 0;
            int loaded = _cargoLoaded.ContainsKey(type) ? _cargoLoaded[type] : 0;
            int total = current + expenses + loaded;
            
            if (currentText != null)
                currentText.text = current.ToString();
            
            if (expensesText != null)
                expensesText.text = expenses.ToString();
            
            if (loadedText != null)
                loadedText.text = loaded.ToString();
            
            if (totalText != null)
                totalText.text = total.ToString();
        }

        private void OnDestroy()
        {
            if (MarsManager.Instance != null)
            {
                MarsManager.Instance.OnColonyStateChanged -= OnColonyStateChanged;
                MarsManager.Instance.OnPreviewUpdated -= OnPreviewUpdated;
            }
            
            if (CargoManager.Instance != null)
            {
                CargoManager.Instance.OnResourcesChanged -= OnCargoChanged;
                CargoManager.Instance.OnCargoCleared -= OnCargoCleared;
            }
        }
    }
}

