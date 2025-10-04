using UnityEngine;
using TMPro;
using GameApplication.Gameplay.Managers;

namespace GameApplication.UI
{
    public class TurnCounterView : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI turnText;

        private void Start()
        {
            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.OnTurnChanged += UpdateTurn;
                UpdateTurn(0);
            }
        }

        private void UpdateTurn(int currentTurn)
        {
            if (turnText != null && GameFlowManager.Instance != null && GameFlowManager.Instance.CurrentGameState != null)
            {
                int totalTurns = GameFlowManager.Instance.CurrentGameState.TurnsToWin;
                turnText.text = $"Ход {currentTurn} из {totalTurns}";
            }
        }

        private void OnDestroy()
        {
            if (GameFlowManager.Instance != null)
                GameFlowManager.Instance.OnTurnChanged -= UpdateTurn;
        }
    }
}

