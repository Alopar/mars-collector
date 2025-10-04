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
        public GameObject eventPanel;

        [Header("Settings")]
        public float displayDuration = 3f;

        private void Start()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.OnEventTriggered += ShowEvent;
            }
            
            if (eventPanel != null)
                eventPanel.SetActive(false);
        }

        private void ShowEvent(TurnEvent turnEvent)
        {
            if (titleText != null)
                titleText.text = turnEvent.title;
            
            if (descriptionText != null)
                descriptionText.text = turnEvent.description;
            
            if (eventPanel != null)
            {
                eventPanel.SetActive(true);
                
                if (displayDuration > 0)
                {
                    Invoke(nameof(HideEvent), displayDuration);
                }
            }
        }

        private void HideEvent()
        {
            if (eventPanel != null)
                eventPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
                EventManager.Instance.OnEventTriggered -= ShowEvent;
        }
    }
}

