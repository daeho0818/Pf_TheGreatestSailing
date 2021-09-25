using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class RunningGameLifeUIPresenter : IDisposable
{
    private readonly ServiceLocator _serviceLocator;
    private readonly ILifeGameModel _gameModel;

    private readonly RunningGameLifeUIView _view;

    private readonly Queue<int> _currentLifeAnimationQueue;

    private readonly CancellationTokenSource _cts;

    public RunningGameLifeUIPresenter(ServiceLocator serviceLocator, ILifeGameModel gameModel)
    {
        _serviceLocator = serviceLocator;

        _gameModel = gameModel;
        _gameModel.Life.OnValueChanged += OnLifeChanged;

        _view = ResourcesUtility.Instantiate<RunningGameLifeUIView>("RunningGame/Prefabs/UI/LifeUIView", UICanvas.Instance.transform);

        _cts = new CancellationTokenSource();
        _currentLifeAnimationQueue = new Queue<int>();

        _ = QueueAnimationAsync();

        OnLifeChanged(_gameModel.Life.Value);
    }

    private void OnLifeChanged(int currentLife)
    {
        _currentLifeAnimationQueue.Enqueue(currentLife);
    }

    private async Task QueueAnimationAsync()
    {
        while (!_cts.IsCancellationRequested)
        {
            while (_currentLifeAnimationQueue.Any())
            {
                var life = _currentLifeAnimationQueue.Dequeue();
                await _view.ApplyAsync(new RunningGameLifeUIView.Entity(life));
            }

            await Task.Yield();
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();

        if (_view)
        {
            UnityEngine.Object.Destroy(_view.gameObject);
        }
    }
}
