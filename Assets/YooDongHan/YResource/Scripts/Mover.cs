using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [System.Serializable]
    public class EffectBySpeed
    {
        [field: SerializeField]
        public float Speed { get; private set; }
        [field: SerializeField]
        public ParticleSystem Effect { get; private set; }
    }

    [SerializeField]
    private EffectBySpeed[] _effects;

    private Dictionary<float, ParticleSystem> _effectBySpeed;

    private Boat _boat = null;
    private Rigidbody _rigidbody = null;

    private void Awake()
    {
        _boat = GetComponent<Boat>();
        _rigidbody = GetComponent<Rigidbody>();
        _effectBySpeed = _effects.OrderBy(x => x.Speed).ToDictionary(x => x.Speed, x => x.Effect);
    }

    private ParticleSystem _target;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 moveVector = transform.forward * _boat.MoveSpeed;
            _rigidbody.AddForce(moveVector);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 moveVector = -transform.forward * _boat.MoveSpeed;
            _rigidbody.AddForce(moveVector);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 rotationVector = -transform.up * _boat.RotationSpeed;
            _rigidbody.AddTorque(rotationVector);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 rotationVector = transform.up * _boat.RotationSpeed;
            _rigidbody.AddTorque(rotationVector);
        }

        var target = _effectBySpeed
            .Last(x => x.Key <= _rigidbody.velocity.magnitude)
            .Value;

        if (target != _target)
        {
            foreach (var effect in _effectBySpeed.Values)
            {
                effect.Stop();
            }

            target.Play();
            _target = target;
        }
    }
}
