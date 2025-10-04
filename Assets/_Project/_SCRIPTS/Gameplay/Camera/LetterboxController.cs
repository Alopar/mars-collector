using UnityEngine;

namespace Domain.Camera
{
    [ExecuteAlways]
    public class LetterboxController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private RectTransform _topPlate;
        [SerializeField] private RectTransform _leftPlate;
        [SerializeField] private RectTransform _rightPlate;
        [SerializeField] private RectTransform _bottomPlate;

        [Space(10)]
        [SerializeField] private RectTransform _uiContainer;
        #endregion

        #region METHODS PRIVATE
        private bool ValidateReferences()
        {
            return _uiContainer.parent as RectTransform;
        }

        private (float totalWidth, float totalHeight, float contentWidth, float contentHeight) CalculateContainerDimensions()
        {
            var parent = _uiContainer.parent as RectTransform;
            return (parent.rect.width, parent.rect.height, _uiContainer.rect.width, _uiContainer.rect.height);
        }

        private (float horizontal, float vertical) CalculatePadding((float totalWidth, float totalHeight, float contentWidth, float contentHeight) dimensions)
        {
            var horizontalPadding = Mathf.Max(0, (dimensions.totalWidth - dimensions.contentWidth) / 2f);
            var verticalPadding = Mathf.Max(0, (dimensions.totalHeight - dimensions.contentHeight) / 2f);
            return (horizontalPadding, verticalPadding);
        }

        private void UpdateBars((float horizontal, float vertical) padding)
        {
            _leftPlate.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, padding.horizontal);
            _rightPlate.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, padding.horizontal);
            _topPlate.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, padding.vertical);
            _bottomPlate.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, padding.vertical);
        }
        #endregion

        #region UNITY CALLBACKS
        private void LateUpdate()
        {
            if (!ValidateReferences()) return;
        
            var dimensions = CalculateContainerDimensions();
            var padding = CalculatePadding(dimensions);
            UpdateBars(padding);
        }
        #endregion
    }
}
