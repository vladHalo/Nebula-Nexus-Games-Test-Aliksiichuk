using System;
using UnityEngine;

namespace Core.Scripts.Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private float _height;
        [SerializeField] private float _speed, _lerpTime;
        [SerializeField] private Vector3 _offset;

        private float _time;
        private float _minScale, _maxScale;
        private BezierCurve _bezierCurve;
        [SerializeField] private Transform _firstPoint, _lastPoint;

        public ItemType _itemType;
        public ItemMoveType _itemMoveType;
        public Action FinishMoveBezier;

        private void Start()
        {
            _bezierCurve = GameManager.instance.bezierCurve;
            _minScale = transform.localScale.x / 2;
            _maxScale = transform.localScale.x;
        }

        private void FixedUpdate()
        {
            if (_itemMoveType == ItemMoveType.Follow)
                FollowMove();
        }

        private void Update()
        {
            if (_itemMoveType == ItemMoveType.Bezier)
                BezierMove();
        }

        public void SetPointMove(Transform firstPoint, Transform lastPoint)
        {
            _firstPoint = firstPoint;
            _lastPoint = lastPoint;
            _time = 0;
        }

        private void FollowMove()
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _lastPoint.position + _offset, _lerpTime * Time.deltaTime);

            _time = Mathf.Lerp(_time, 1f, _lerpTime * Time.deltaTime);

            if (_time > 0.99f)
            {
                _time = 0f;
            }
        }

        private void BezierMove()
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
                _time = 0;
                transform.localScale = Vector3.one;
                transform.position = new Vector3(_lastPoint.position.x + _offset.x, _lastPoint.position.y + _offset.y,
                    _lastPoint.position.z);

                _itemMoveType = ItemMoveType.Follow;
                FinishMoveBezier?.Invoke();
                FinishMoveBezier = null;
            }
        }
    }
}