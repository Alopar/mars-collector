using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameApplication.Gameplay.Managers;
using GameApplication.Gameplay.Models;

namespace GameApplication.UI
{
    public class ShipView : MonoBehaviour
    {
        [Header("Buttons")]
        public Button weaponsButton;
        public Button suppliesButton;
        public Button peopleButton;
        public Button launchButton;
        
        [Header("Display")]
        public TextMeshProUGUI cargoText;

        private void Start()
        {
            weaponsButton.onClick.AddListener(() => LoadWeapons());
            suppliesButton.onClick.AddListener(() => LoadSupplies());
            peopleButton.onClick.AddListener(() => LoadPeople());
            launchButton.onClick.AddListener(() => LaunchShip());

            if (ShipManager.Instance != null)
            {
                ShipManager.Instance.OnCargoChanged += UpdateCargoDisplay;
                
                if (ShipManager.Instance.CurrentCargo != null)
                {
                    UpdateCargoDisplay(0, ShipManager.Instance.CurrentCargo.MaxCapacity);
                }
            }

            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.OnPhaseChanged += HandlePhaseChange;
            }
        }

        private void LoadWeapons()
        {
            if (ShipManager.Instance != null)
                ShipManager.Instance.LoadResource(ResourceType.Weapons, 1);
        }

        private void LoadSupplies()
        {
            if (ShipManager.Instance != null)
                ShipManager.Instance.LoadResource(ResourceType.Supplies, 1);
        }

        private void LoadPeople()
        {
            if (ShipManager.Instance != null)
                ShipManager.Instance.LoadResource(ResourceType.People, 1);
        }

        private void LaunchShip()
        {
            if (ShipManager.Instance != null)
                ShipManager.Instance.LaunchShip();
        }

        private void UpdateCargoDisplay(int current, int max)
        {
            cargoText.text = $"Загружено: {current}/{max}";
            launchButton.interactable = current > 0;
        }

        private void HandlePhaseChange(GamePhase phase)
        {
            bool canInteract = phase == GamePhase.Loading;
            weaponsButton.interactable = canInteract;
            suppliesButton.interactable = canInteract;
            peopleButton.interactable = canInteract;
            
            if (ShipManager.Instance != null && ShipManager.Instance.CurrentCargo != null)
            {
                launchButton.interactable = canInteract && ShipManager.Instance.CurrentCargo.GetTotalResources() > 0;
            }
            else
            {
                launchButton.interactable = false;
            }
        }

        private void OnDestroy()
        {
            if (ShipManager.Instance != null)
                ShipManager.Instance.OnCargoChanged -= UpdateCargoDisplay;
            
            if (GameFlowManager.Instance != null)
                GameFlowManager.Instance.OnPhaseChanged -= HandlePhaseChange;
        }
    }
}

