using DG.Tweening;
using UnityEngine;

namespace Core.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BackpackPlayer))]
    public class MovementPlayer : MonoBehaviour
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

        private void FixedUpdate()
        {
            Movement();
        }

        private void Movement()
        {
            Vector3 moveVector = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
            _rigidbody.velocity = new Vector3(moveVector.x * _speed, _rigidbody.velocity.y, moveVector.z * _speed);

            if (Vector3.Angle(Vector3.forward, moveVector) > 1f || Vector3.Angle(Vector3.forward, moveVector) == 0)
            {
                Vector3 direct =
                    Vector3.RotateTowards(transform.forward, moveVector, _speed * 4f * Time.fixedDeltaTime, 0.0f);
                _rigidbody.rotation = Quaternion.LookRotation(direct);
            }

            var floatSpeed = _rigidbody.velocity.magnitude / _speed;
            _animator.SetFloat(Str.MoveSpeed, floatSpeed);
        }

        //Change animation hands with items or not
        public void SetLayerWeight(int layerWeight, float value, float duration = 0)
        {
            DOTween.Kill(this);

            duration = Mathf.Max(duration, 0);
            if (duration > 0)
            {
                var curr = _animator.GetLayerWeight(layerWeight);
                DOVirtual.Float(curr, value, duration,
                        w => _animator.SetLayerWeight(layerWeight, w))
                    .SetEase(Ease.Linear);
            }
            else _animator.SetLayerWeight(layerWeight, value);
        }
    }
}