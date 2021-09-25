using System;
using UnityEngine;

public class BoxView : MonoBehaviour
{
    public readonly struct Entity
    {
        public bool ApplyGravity { get; }

        public Entity(bool applyGravity)
        {
            ApplyGravity = applyGravity;
        }
    }

    public event Action OnBoxStopped = () => { };

    [SerializeField]
    private AudioSource _sound;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Rigidbody2D _rigidBody;

    public float ExtentsX => _spriteRenderer.size.x / 2;

    private bool _checkBoxStopped;
    private bool _checkStability;

    private float _delaySeconds;

    private bool _soundPlayed;

    public void Apply(Entity entity)
    {
        var gravity = entity.ApplyGravity ? 3f : 0;
        _rigidBody.gravityScale = gravity;
    }

    private void Update()
    {
        if (_checkStability)
        {
            _delaySeconds += Time.deltaTime;
        }
        else
        {
            _delaySeconds = 0;
        }

        if (_delaySeconds > 1f)
        {
            OnBoxStopped.Invoke();
            _checkBoxStopped = true;
            _checkStability = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_soundPlayed) return;

        _sound.Play();
        _soundPlayed = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _checkStability = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_checkBoxStopped) return;

        _checkStability = _rigidBody.velocity.x < 0.2f && _rigidBody.velocity.y < 0.2f;
    }
}
