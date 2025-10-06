using GameApplication.Gameplay.Managers;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameApplication.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject gameOverPanel;
        public TextMeshProUGUI messageText;
        public TextMeshProUGUI statsText;
        public Button restartButton;

        public GameObject videoVictory;
        public GameObject videoSecret;

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
            messageText.gameObject.SetActive(true);
            statsText.gameObject.SetActive(true);

            if (victory && MissionsManager.Instance.CurrentMissionIndex == 5)
            {
                ShowSecret();
            }
            else if (victory && GameFlowManager.Instance.CurrentGameState.CurrentTurn >= GameFlowManager.Instance.Config.TurnsToWin)
            {
                ShowVictory();
            }



            if (messageText != null)
            {
                messageText.text = message;
                // messageText.color = victory ? victoryColor : defeatColor;
            }

            if (statsText != null)
            {
                var gameState = GameFlowManager.Instance.CurrentGameState;
                statsText.text = $"STATISTICS\n" +
                                $"Turns survived: {gameState.CurrentTurn}\n" +
                                $"Ships dispatched: {gameState.ShipsSent}";
            }

            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
        }

        private void ShowVictory()
        {
            videoVictory.SetActive(true);
            restartButton.gameObject.SetActive(false);
            messageText.gameObject.SetActive(false);
            statsText.gameObject.SetActive(false);

            StartCoroutine(WaitForVictoryVideoEnd());
        }

        private void ShowSecret()
        {
            videoSecret.SetActive(true);
            restartButton.gameObject.SetActive(false);
            messageText.gameObject.SetActive(false);
            statsText.gameObject.SetActive(false);

            StartCoroutine(WaitForSecretVideoEnd());
        }

        private IEnumerator WaitForVictoryVideoEnd()
        {
            yield return new WaitForSeconds(25);
            videoVictory.SetActive(false);
            restartButton.gameObject.SetActive(true);
        }

        private IEnumerator WaitForSecretVideoEnd()
        {
            yield return new WaitForSeconds(40);
            videoSecret.SetActive(false);
            restartButton.gameObject.SetActive(true);
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

