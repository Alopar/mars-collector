using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.SceneLoader
{
    public class SceneLoader : MonoBehaviour
    {
        #region FIELDS PRIVATE
        #endregion

        #region METHODS PRIVATE
        private IEnumerator LoadScene(AsyncOperation operation, Action callback)
        {
            while (!operation.isDone)
            {
                yield return null;
            }

            callback?.Invoke();
        }
        #endregion

        #region METHODS PUBLIC
        public void Load(string name, Action callback = null)
        {
            var operation = SceneManager.LoadSceneAsync(name);
            StartCoroutine(LoadScene(operation, callback));
        }

        public void Load(int index, Action callback = null)
        {
            var operation = SceneManager.LoadSceneAsync(index);
            StartCoroutine(LoadScene(operation, callback));
        }
        #endregion
    }
}
