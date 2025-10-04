using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameApplication.Gameplay.Managers;

namespace GameApplication.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject gameOverPanel;
        public TextMeshProUGUI messageText;
        public TextMeshProUGUI statsText;
        public Button restartButton;

        [Header("Colors")]
        public Color victoryColor = Color.green;
        public Color defeatColor = Color.red;

        private void Start()
        {
            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.OnGameEnded += ShowGameOver;
            }
            
            if (restartButton != null)
            {
                restartButton.onClick.AddListener(Restart);
            }
            
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);
        }

        private void ShowGameOver(bool victory, string message)
        {
            if (messageText != null)
            {
                messageText.text = message;
                messageText.color = victory ? victoryColor : defeatColor;
            }

            if (statsText != null)
            {
                var gameState = GameFlowManager.Instance.CurrentGameState;
                statsText.text = $"Статистика:\n" +
                                $"Ходов прожито: {gameState.CurrentTurn}\n" +
                                $"Кораблей отправлено: {gameState.ShipsSent}";
            }

            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
        }

        private void Restart()
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);
            
            if (GameFlowManager.Instance != null)
                GameFlowManager.Instance.RestartGame();
        }

        private void OnDestroy()
        {
            if (GameFlowManager.Instance != null)
                GameFlowManager.Instance.OnGameEnded -= ShowGameOver;
        }
    }
}

