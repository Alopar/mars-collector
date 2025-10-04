using System;
using UnityEngine;

namespace Utility
{
    public class RadiusVisualizatorGizmo : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField, Range(6, 360)] private int _precision = 90;
        [SerializeField] private Color _color = Color.white;

        [Space(10)]
        [SerializeField] private GameObject _objectWithRadius;
        #endregion

        #region FIELDS PRIVATE
        private float _radius;
        private IHasRadius _objectCache;
        #endregion

        #region METHODS PUBLIC
        public void SetRadius(float radius)
        {
            _radius = radius;
        }
        #endregion

        #region METHODS PRIVATE
        private void GetRadiusObject()
        {
            if (!_objectWithRadius) return;
            if (!_objectWithRadius.TryGetComponent<IHasRadius>(out var component)) return;
            _objectCache = component;
        }
        
        private void GetRadius()
        {
            if (_objectCache is null) return;
            _radius = _objectCache.Radius;
        }

        private void DrawCircle()
        {
            if (_radius == 0) return;

            var points = GetPointsOnRadius();
            for (var i = 1; i < points.Length; i++)
            {
                Debug.DrawLine(points[i - 1], points[i], _color);
            }
            Debug.DrawLine(points[^1], points[0], _color);
        }

        private Vector3[] GetPointsOnRadius()
        {
            var points = new Vector3[_precision];
            for (var i = 0; i < _precision; i++)
            {
                var angle = (360f / _precision) * i;
                var point = GetPointOnCircleByAngle(transform.position, angle, _radius);
                points[i] = point;
            }

            return points;
        }

        private Vector3 GetPointOnCircleByAngle(Vector3 center, float angle, float radius)
        {
            var x = Mathf.Sin(angle * Mathf.Deg2Rad) * radius + center.x;
            var y = Mathf.Cos(angle * Mathf.Deg2Rad) * radius + center.y;
            var z = center.z;
            return new Vector3(x, y, z);
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            GetRadiusObject();
        }

        private void OnDrawGizmos()
        {
            GetRadius();
            DrawCircle();
        }
        #endregion
    }
}
