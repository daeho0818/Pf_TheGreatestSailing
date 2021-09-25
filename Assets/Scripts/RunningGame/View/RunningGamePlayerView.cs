using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RunningGamePlayerView : MonoBehaviour
{
    public readonly struct Entity
    {
        public float SpeedCoefficient { get; }

        public Entity(float speedCoefficient)
        {
            SpeedCoefficient = speedCoefficient;
        }
    }

    public event Action<RunningGameObstaclePositionState> OnObstacleCollide = name => { };

    [SerializeField]
    private Animator _playerAnimator;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private float _jumpPower;
    [SerializeField]
    private float _jumpDuration;

    private IReadOnlyList<AnimatorStateMachineBehaviour> _behaviours;

    private float _speedCoefficient = 1;

    private void Awake()
    {
        _behaviours = _playerAnimator.GetBehaviours<AnimatorStateMachineBehaviour>();
    }

    public void Apply(Entity entity)
    {
        _speedCoefficient = entity.SpeedCoefficient;
        _playerAnimator.SetFloat("Speed", _speedCoefficient);
    }

    public async Task JumpAsync()
    {
        _audioSource.Play();
        _playerAnimator.SetTrigger("Jump");

        var jumpTween = transform.DOJump(transform.position, _jumpPower, 1, _jumpDuration).AsyncWaitForCompletion();

        await jumpTween;

        _playerAnimator.SetTrigger("JumpToRun");
    }

    public void Unslide()
    {
        _playerAnimator.SetTrigger("Unslide");
    }

    public void Slide()
    {
        _playerAnimator.SetTrigger("Slide");
    }

    public async Task HurtAsync()
    {
        await SetTriggerAndWaitToFinishAsync("Hurt", 1);
    }

    public async Task DeadAsync()
    {
        await SetTriggerAndWaitToFinishAsync("Dead", 1);
    }

    private async Task SetTriggerAndWaitToFinishAsync(string name, int layerIndex)
    {
        _playerAnimator.SetTrigger(name);

        await _behaviours[layerIndex].OnStateExitAsync(name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var view = collision.gameObject.GetComponent<RunningGameObstacleElementView>();
        if (view != null)
        {
            OnObstacleCollide.Invoke(view.State);
        }
    }
}
