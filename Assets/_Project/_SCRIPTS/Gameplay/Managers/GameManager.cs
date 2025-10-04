using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameApplication.Gameplay.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Button("⭐ START GAME ⭐")]
        public void StartGame()
        {
            StartCoroutine(DelayCall(0.25f, () => { SceneManager.LoadScene(2); }));
        }

        [Button("💢 RESTART GAME 💢")]
        public void RestartGame()
        {
            StartCoroutine(DelayCall(0.25f, () => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }));
        }

        [Button("🎲 START MENU 🎲")]
        public void StartMenu()
        {
            StartCoroutine(DelayCall(0.25f, () => { SceneManager.LoadScene(1); }));
        }

        [Button("❔ SHOW TUTORIAL ❔")]
        public void ShowTutorial()
        {
            StartCoroutine(DelayCall(0.25f, () => { SceneManager.LoadScene(2); }));
        }

        [Button("💤 PAUSE GAME 💤")]
        public void PauseGame()
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Time.timeScale = 1.0f;
        }

        private IEnumerator DelayCall(float seconds, Action callback)
        {
            yield return new WaitForSecondsRealtime(seconds);
            callback?.Invoke();
        }
    }
}
