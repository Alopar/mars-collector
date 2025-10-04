using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField, Range(0, 10)] private float _speed = 5f;

        [Space(10)]
        [SerializeField] private InputActionReference _moveInputAction;
        [SerializeField] private InputActionReference _lookInputAction;

        [Space(10)]
        [SerializeField] private Transform _view;
        [SerializeField] private Animator _animator;
        #endregion

        #region FIELDS PRIVATE
        private UnityEngine.Camera _camera;
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            _camera = UnityEngine.Camera.main;
        }

        private void Update()
        {
            if (Time.timeScale == 0) return;

            Move();
            Rotate();
        }
        #endregion

        #region METHODS PRIVATE
        private void Move()
        {
            var input = _moveInputAction.action.ReadValue<Vector2>();
            if (input == Vector2.zero)
            {
                _animator.SetBool("IsMoved", false);
                return;
            }

            _animator.SetBool("IsMoved", true);
            var direction = new Vector3(input.x, input.y, 0f);
            transform.position += direction.normalized * (_speed * Time.deltaTime);
        }

        private void Rotate()
        {
            switch (GetLookDirection().x)
            {
                case < 0: _view.rotation = Quaternion.Euler(0f, 180f, 0f);
                    break;
                default: _view.rotation = Quaternion.Euler(0f, 0f, 0f);
                    break;
            }
        }

        private Vector2 GetLookDirection()
        {
            var playerAtScreen = _camera.WorldToScreenPoint(transform.position);
            var cameraPosition = new Vector2(playerAtScreen.x, playerAtScreen.y);
            var mousePosition = _lookInputAction.action.ReadValue<Vector2>();

            return (mousePosition - cameraPosition).normalized;;
        }
        #endregion
    }
}
