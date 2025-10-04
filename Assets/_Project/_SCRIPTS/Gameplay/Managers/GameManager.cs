using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        #endregion

        #region FIELDS PRIVATE
        #endregion

        #region PROPERTIES
        #endregion
        
        #region METHODS PUBLIC
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
            StartCoroutine(DelayCall(0.25f, () => { SceneManager.LoadScene(0); }));
        }

        [Button("❔ SHOW TUTORIAL ❔")]
        public void ShowTutorial()
        {
            StartCoroutine(DelayCall(0.25f, () => { SceneManager.LoadScene(1); }));
        }

        [Button("💤 PAUSE GAME 💤")]
        public void PauseGame()
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Time.timeScale = 1.0f;
        }
        #endregion

        #region COROUTINES
        private IEnumerator DelayCall(float seconds, Action callback)
        {
            yield return new WaitForSecondsRealtime(seconds);
            callback?.Invoke();
        }
        #endregion
    }
}
