using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class RunningGamePlayerPresenter : IDisposable
{
    private readonly RunningGameModel _model;
    private readonly RunningGamePlayerView _view;

    private readonly List<IDisposable> _disposables = new List<IDisposable>();

    private readonly HashSet<KeyCode> _keyHashSets = new HashSet<KeyCode>();

    public RunningGamePlayerPresenter(ServiceLocator _serviceLocator, RunningGameModel model)
    {
        _model = model;
        _model.Life.OnValueChanged += currentLife => _ = OnLifeChangedAsync(currentLife);

        _view = _serviceLocator.GetFromSceneObject<RunningGamePlayerView>();
        _view.OnObstacleCollide += OnObstacleCollide;

        MessageBroker.Default.Receive<RunningGameMessages.KeyDownMessage>(x => _ = OnReceiveKeyDownAsync(x))
            .AddTo(_disposables);

        MessageBroker.Default.Receive<RunningGameMessages.KeyUpMessage>(OnReceiveKeyUp)
            .AddTo(_disposables);

        MessageBroker.Default.Receive<RunningGameMessages.CoefficientChangedMessage>(x => _view.Apply(new RunningGamePlayerView.Entity(x.SpeedCoefficient)))
            .AddTo(_disposables);
    }

    private async Task OnLifeChangedAsync(int currentLife)
    {
        if (currentLife > 0)
        {
            MessageBroker.Default.Publish(new RunningGameMessages.SystemSpeedCoefficientChangedMessage(0));
            _model.ChangeState(RunningGamePlayerState.Hurting);

            await _view.HurtAsync();

            MessageBroker.Default.Publish(new RunningGameMessages.SystemSpeedCoefficientChangedMessage(1));

            _model.ChangeState(RunningGamePlayerState.Invinsible);

            await Task.Delay(100);

            _model.ChangeState(RunningGamePlayerState.Normal);
        }
        else
        {
            _model.ChangeState(RunningGamePlayerState.Dead);
            MessageBroker.Default.Publish(new RunningGameMessages.SystemSpeedCoefficientChangedMessage(0));

            await _view.DeadAsync();
        }
    }

    private void OnObstacleCollide(RunningGameObstaclePositionState state)
    {
        _model.CheckDamaged(state);
    }

    private void OnReceiveKeyUp(RunningGameMessages.KeyUpMessage msg)
    {
        _keyHashSets.Remove(msg.KeyCode);

        switch (msg.KeyCode)
        {
            case KeyCode.S when _model.PlayerState.Value is RunningGamePlayerState.Ducking:
                _model.ChangeState(RunningGamePlayerState.Normal);
                _view.Unslide();
                break;
        }
    }

    private async Task OnReceiveKeyDownAsync(RunningGameMessages.KeyDownMessage msg)
    {
        if (_keyHashSets.Any()) return;

        _keyHashSets.Add(msg.KeyCode);

        switch (msg.KeyCode)
        {
            case KeyCode.W when _model.PlayerState.Value is RunningGamePlayerState.Normal:
                _model.ChangeState(RunningGamePlayerState.Jumping);
                _ = _view.JumpAsync();
                await Task.Delay(300);
                _model.ChangeState(RunningGamePlayerState.Normal);
                break;
            case KeyCode.S when _model.PlayerState.Value is RunningGamePlayerState.Normal:
                _model.ChangeState(RunningGamePlayerState.Ducking);
                _view.Slide();
                break;
        };
    }

    public void Dispose()
    {
        _view.OnObstacleCollide -= OnObstacleCollide;

        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }

        _disposables.Clear();
    }
}
