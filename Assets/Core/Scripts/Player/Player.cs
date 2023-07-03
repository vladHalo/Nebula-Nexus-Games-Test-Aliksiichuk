using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;
    private Animator _animator;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        var moveVector = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        _rigidbody.velocity = new Vector3(moveVector.x * _speed, _rigidbody.velocity.y, moveVector.z * _speed);

        if (Vector3.Angle(Vector3.forward, moveVector) > 1f || Vector3.Angle(Vector3.forward, moveVector) == 0)
        {
            Vector3 direct =
                Vector3.RotateTowards(transform.forward, moveVector, _speed * 4f * Time.fixedDeltaTime, 0.0f);
            _rigidbody.rotation = Quaternion.LookRotation(direct);
        }

        float speed = _rigidbody.velocity.magnitude / _speed;
        _animator.SetFloat("Move", speed);
    }
}