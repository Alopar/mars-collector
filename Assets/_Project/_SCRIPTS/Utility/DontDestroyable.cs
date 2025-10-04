using System;
using UnityEngine;

namespace GameApplication.Utility
{
    public class DontDestroyable : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
