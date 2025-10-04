using UnityEngine;

namespace Gameplay.Camera
{
    [ExecuteAlways]
    public class CameraAspectLimiter : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private float _minAspectRatio;
        [SerializeField] private float _maxAspectRatio;

        [Space(10)]
        [SerializeField] private UnityEngine.Camera _camera;
        #endregion

        #region METHODS PRIVATE
        private void UpdateViewport()
        {
            var windowAspect = (float)Screen.width / (float)Screen.height;
            if (windowAspect >= _minAspectRatio && windowAspect <= _maxAspectRatio)
            {
                _camera.rect = new Rect(0, 0, 1.0f, 1.0f);
                return;
            }

            var targetAspect = windowAspect < _minAspectRatio ? _minAspectRatio : _maxAspectRatio;
            ApplyAspectRatio(targetAspect);
        }

        private void ApplyAspectRatio(float targetAspect)
        {
            var windowAspect = (float)Screen.width / (float)Screen.height;
            var scaleHeight = windowAspect / targetAspect;

            if (scaleHeight < 1.0f)
            {
                var rect = new Rect(0, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);
                _camera.rect = rect;
            }
            else
            {
                var scaleWidth = 1.0f / scaleHeight;
                var rect = new Rect((1.0f - scaleWidth) / 2.0f, 0, scaleWidth, 1.0f);
                _camera.rect = rect;
            }
        }
        #endregion

        #region UNITY CALLBACKS
        private void Update()
        {
            UpdateViewport();
        }
        #endregion
    }
}
