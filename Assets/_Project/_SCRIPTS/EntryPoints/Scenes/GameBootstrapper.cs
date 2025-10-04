using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameApplication.EntryPoints.Scenes
{
    [DefaultExecutionOrder(-100)]
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private string _nextSceneName;

        private void Start()
        {
            SceneManager.LoadScene(_nextSceneName);
        }
    }
}
