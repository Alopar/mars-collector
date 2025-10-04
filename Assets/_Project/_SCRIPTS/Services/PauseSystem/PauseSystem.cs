
using UnityEngine;

namespace Services.PauseSystem
{
    public class PauseSystem : MonoBehaviour
    {
        #region FIELDS PRIVATE
        private bool _isPaused;
        #endregion

        #region CONSTRUCTOR
        public PauseSystem()
        {
            // _browserFocusObserver.OnApplicationFocusChanged += ApplicationFocusChanged;
            // _browserFocusObserver.OnApplicationPauseChanged += ApplicationPauseChanged;
        }
        #endregion

        #region METHODS PUBLIC
        public void Pause()
        {
            // if (_isPaused) return;
            // _isPaused = true;
            //
            // Time.timeScale = 0f;
            // if (type == PauseType.Regular) return;
            //
            // _audioService?.PauseMusic();
        }

        public void Resume()
        {
            // if (!_isPaused) return;
            // _isPaused = false;
            //
            // Time.timeScale = 1f;
            // _audioService?.ResumeMusic();
        }
        #endregion

        #region METHODS PRIVATE
        private void ApplicationFocusChanged(bool state)
        {
            // if (!state)
            // {
            //     Pause(PauseType.Deep);
            //     return;
            // }
            //
            // Resume();
        }

        private void ApplicationPauseChanged(bool state)
        {
            // if (state)
            // {
            //     Pause(PauseType.Deep);
            //     return;
            // }
            //
            // Resume();
        }
        #endregion
    }
}
