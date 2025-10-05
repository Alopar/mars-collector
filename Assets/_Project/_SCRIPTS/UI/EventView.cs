using UnityEngine;
using TMPro;
using GameApplication.Gameplay.Managers;
using GameApplication.Gameplay.Models;

namespace GameApplication.UI
{
    public class EventView : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;

        private void Start()
        {
            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.OnTurnChanged += OnTurnChanged;
            }
            
            UpdateEventDisplay();
        }
        
        private void OnTurnChanged(int turn)
        {
            UpdateEventDisplay();
        }

        private void UpdateEventDisplay()
        {
            if (EventManager.Instance == null)
                return;
            
            TurnEvent currentEvent = EventManager.Instance.GetCurrentEvent();
            
            if (currentEvent != null)
            {
                if (titleText != null)
                    titleText.text = currentEvent.title;
                
                if (descriptionText != null)
                    descriptionText.text = currentEvent.description;
            }
            else
            {
                if (titleText != null)
                    titleText.text = "";
                
                if (descriptionText != null)
                    descriptionText.text = "";
            }
        }

        private void OnDestroy()
        {
            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.OnTurnChanged -= OnTurnChanged;
            }
        }
    }
}

