using UnityEngine;
using UnityEngine.UI;

namespace Domain.Camera
{
    [ExecuteAlways]
    public class ScreenAspectLimiter : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private float _minAspectRatio;
        [SerializeField] private float _maxAspectRatio;

        [Space(10)]
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private RectTransform _rectTransform;
        #endregion

        #region METHODS PRIVATE
        private void UpdateCanvasScalerMatch(float currentAspect)
        {
            var matchValue = 0f;
            if (currentAspect >= _maxAspectRatio)
            {
                matchValue = 1f;
            }
            else if (currentAspect > _minAspectRatio)
            {
                matchValue = Mathf.Lerp(0f, 1f, (currentAspect - _minAspectRatio) / (_maxAspectRatio - _minAspectRatio));
            }
            _canvasScaler.matchWidthOrHeight = matchValue;
        }

        private bool IsAspectWithinBounds(float aspect)
        {
            return aspect >= _minAspectRatio && aspect <= _maxAspectRatio;
        }

        private (float width, float height) CalculateRectDimensions(float parentWidth, float parentHeight, float parentAspect)
        {
            var targetAspect = parentAspect < _minAspectRatio ? _minAspectRatio : _maxAspectRatio;

            float width, height;
            if (parentAspect > targetAspect)
            {
                height = parentHeight;
                width = height * targetAspect;
            }
            else
            {
                width = parentWidth;
                height = width / targetAspect;
            }

            return (width, height);
        }

        private void SetRectTransformSize(float width, float height)
        {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            _rectTransform.anchoredPosition = Vector2.zero;
        }

        private (float width, float height, float aspect) GetParentRectDimensions()
        {
            var parentRect = ((RectTransform)_rectTransform.parent).rect;
            var width = parentRect.width;
            var height = parentRect.height;
            var aspect = width / height;
            return (width, height, aspect);
        }
        #endregion
    
        #region UNITY CALLBACKS
        private void LateUpdate()
        {
            var (parentWidth, parentHeight, parentAspect) = GetParentRectDimensions();
            UpdateCanvasScalerMatch(parentAspect);

            if (IsAspectWithinBounds(parentAspect))
            {
                SetRectTransformSize(parentWidth, parentHeight);
                return;
            }

            var (width, height) = CalculateRectDimensions(parentWidth, parentHeight, parentAspect);
            SetRectTransformSize(width, height);
        }
        #endregion
    }
}
