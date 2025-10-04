using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Camera
{
    public class CameraController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField, Range(0, 5)] private float _zoomSize = 3f;
        [SerializeField, Range(0, 99)] private float _zoomSpeed = 5f;
        [SerializeField, Range(0, 10)] private int _zoomSteps = 10;

        [Space(10)]
        [SerializeField, Range(0, 5)] private float _offsetDelta = 3f;
        [SerializeField, Range(0, 1)] private float _verticalAspect = 1f;
        [SerializeField, Range(0, 1)] private float _horizontalAspect = 0.25f;
        [SerializeField, Range(0, 5)] private float _offsetSpeed = 1f;

        [Space(10)]
        [SerializeField] private InputActionReference _pointerInputAction;
        [SerializeField] private InputActionReference _wheelInputAction;

        [Space(10)]
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private CinemachineCameraOffset _cinemachineCameraOffset;
        #endregion

        #region FIELDS PRIVATE
        private const float ORTHOGRAPHIC_SIZE = 10f;
        
        private UnityEngine.Camera _camera;
        private int _currentZoomStep;
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            _camera = UnityEngine.Camera.main;
        }

        private void LateUpdate()
        {
            ReadZoomInput();
            ApplyZoom();
            // SetOffset();
        }
        #endregion

        #region METHODS PRIVATE
        private void ReadZoomInput()
        {
            if (!_wheelInputAction.action.WasPerformedThisFrame()) return;

            var wheelDelta = _wheelInputAction.action.ReadValue<Vector2>().y;
            switch (wheelDelta)
            {
                case -1: _currentZoomStep++;
                    break;
                case 1: _currentZoomStep--;
                    break;
            }

            _currentZoomStep = Mathf.Clamp(_currentZoomStep, 0, _zoomSteps);
        }

        private void ApplyZoom()
        {
            var zoomStep = ((ORTHOGRAPHIC_SIZE * _zoomSize) - ORTHOGRAPHIC_SIZE) / _zoomSteps;
            var orthographicSize = _currentZoomStep == 0 ? ORTHOGRAPHIC_SIZE : ORTHOGRAPHIC_SIZE + (zoomStep * _currentZoomStep);
            _cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(_cinemachineCamera.Lens.OrthographicSize, orthographicSize, _offsetSpeed * Time.deltaTime);
        }
        
        private void SetOffset()
        {
            var offset = GetDirection() * _offsetDelta;
            offset.y *= _verticalAspect;
            offset.x *= _horizontalAspect;
            _cinemachineCameraOffset.Offset = Vector2.Lerp(_cinemachineCameraOffset.Offset, offset, _offsetSpeed * Time.deltaTime);
        }

        private Vector2 GetDirection()
        {
            var cameraAtScreen = _camera.WorldToScreenPoint(transform.position);
            var cameraPosition = new Vector2(cameraAtScreen.x, cameraAtScreen.y);
            var mousePosition = _pointerInputAction.action.ReadValue<Vector2>();

            return (mousePosition - cameraPosition).normalized;
        }
        #endregion
    }
}
