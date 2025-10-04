using UnityEngine;

namespace Utility
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererRepeater : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private SpriteRenderer _targetSpriteRenderer;
        #endregion

        #region FIELDS PRIVATE
        private SpriteRenderer _spriteRenderer;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            _spriteRenderer.sprite = _targetSpriteRenderer.sprite;
        }
        #endregion
    }
}
