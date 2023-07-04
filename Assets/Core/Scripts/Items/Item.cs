using System;
using UnityEngine;

namespace Core.Scripts.Items
{
    [RequireComponent(typeof(SmoothLerp))]
    public abstract class Item : MonoBehaviour
    {
        [SerializeField] private float _height;
        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private SmoothLerp _smoothLerp;

        private float _time;
        private float _minScale, _maxScale;
        private BezierCurve _bezierCurve;
        private Transform _firstPoint, _lastPoint;

        private void Start()
        {
            _bezierCurve = GameManager.instance.bezierCurve;
            _minScale = transform.localScale.x / 2;
            _maxScale = transform.localScale.x;
        }

        private void Update()
        {
            MoveBezier();
        }

        public void SetPointMove(Transform firstPoint, Transform lastPoint)
        {
            _firstPoint = firstPoint;
            _lastPoint = lastPoint;
            _time = 0;
            _smoothLerp.enabled = false;
        }

        private void MoveBezier()
        {
            transform.position = _bezierCurve.GetPointOnBezierCurve(
                _firstPoint.position,
                new Vector3(_firstPoint.position.x, _firstPoint.position.y + _height, _firstPoint.position.z),
                new Vector3(_lastPoint.position.x, _lastPoint.position.y + _height, _lastPoint.position.z),
                new Vector3(_lastPoint.position.x + _offset.x, _lastPoint.position.y + _offset.y,
                    _lastPoint.position.z), _time);

            transform.localScale = new Vector3(
                Mathf.Clamp(_time, _minScale, _maxScale),
                Mathf.Clamp(_time, _minScale, _maxScale),
                Mathf.Clamp(_time, _minScale, _maxScale));

            _time += Time.deltaTime * _speed;

            if (_time >= 1)
            {
                _smoothLerp.SetFollow(_lastPoint);
                _smoothLerp.enabled = true;
                enabled = false;
            }
        }
    }
}