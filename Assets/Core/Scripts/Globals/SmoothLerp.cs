using UnityEngine;

public class SmoothLerp : MonoBehaviour
{
    [SerializeField] private float _lerpTime;
    [SerializeField] private Vector3 _offset;

    private float _time;
    [SerializeField] private Transform _parent;
    private Vector3 _position;

    private void Update()
    {
        if (_parent != null) FollowObject();
    }

    public void SetFollow(Transform parent)
    {
        _parent = parent;
        _time = 0;
    }

    private void FollowObject()
    {
        _position = _parent.position;

        transform.position =
            Vector3.MoveTowards(transform.position, _position + _offset, _lerpTime * Time.deltaTime);

        _time = Mathf.Lerp(_time, 1f, _lerpTime * Time.deltaTime);

        if (_time > 0.99f)
        {
            _time = 0f;
        }
    }
}