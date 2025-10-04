using System;
using UnityEngine;

namespace Infrastructure.Application
{
    public class BrowserFocusObserver : MonoBehaviour, IBrowserFocusObserver
    {
        #region EVENTS
        public event Action<bool> OnApplicationFocusChanged;
        public event Action<bool> OnApplicationPauseChanged;
        #endregion

        #region UNITY CALLBACKS
        private void OnApplicationFocus(bool hasFocus)
        {
            OnApplicationFocusChanged?.Invoke(hasFocus);
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            OnApplicationPauseChanged?.Invoke(pauseStatus);
        }
        #endregion
    }
}
