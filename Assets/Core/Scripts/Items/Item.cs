using Lean.Pool;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] private float _height;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _offset;

    private float _time;
    private float _minScale, _maxScale;
    private BezierCurve _bezierCurve;
    private Transform _firstPoint, _lastPoint;
    private ItemStatus _itemStatus;

    private void Start()
    {
        _bezierCurve = GameManager.instance.bezierCurve;
        _minScale = transform.localScale.x / 2;
        _maxScale = transform.localScale.x;
    }

    private void Update()
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

        if (_time >= 1 && _itemStatus == ItemStatus.Disable)
        {
            _time = 1;
            LeanPool.Despawn(this);
        }
    }

    public void SetPointMove(Transform firstPoint, Transform lastPoint, ItemStatus itemStatus = ItemStatus.Enable)
    {
        _firstPoint = firstPoint;
        _lastPoint = lastPoint;
        _itemStatus = itemStatus;
        _time = 0;
    }
}