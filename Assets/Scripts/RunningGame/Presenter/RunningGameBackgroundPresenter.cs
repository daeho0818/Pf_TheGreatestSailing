using System;
using System.Collections.Generic;

public class RunningGameBackgroundPresenter : IDisposable
{
    private readonly RunningGameModel _model;
    private readonly RunningGameBackgroundViews _backgroundView;
    private readonly RunningGameBackgroundPropView _propView;
    private readonly RunningGameObstacleView _obstacleView;

    private readonly List<IDisposable> _disposables = new List<IDisposable>();

    public RunningGameBackgroundPresenter(ServiceLocator _serviceLocator, RunningGameModel model)
    {
        _model = model;

        _backgroundView = _serviceLocator.GetFromSceneObject<RunningGameBackgroundViews>();
        _propView = _serviceLocator.GetFromSceneObject<RunningGameBackgroundPropView>();
        _obstacleView = _serviceLocator.GetFromSceneObject<RunningGameObstacleView>();

        MessageBroker.Default.Receive<RunningGameMessages.CoefficientChangedMessage>(x => RefreshView(x.SpeedCoefficient, x.SpawnCoefficient))
            .AddTo(_disposables);
    }

    private void RefreshView(float speedCoefficient, float spawnCoefficient)
    {
        _backgroundView.Apply(new RunningGameBackgroundViews.Entity(speedCoefficient));
        _propView.Apply(new RunningGameBackgroundPropView.Entity(speedCoefficient, spawnCoefficient));
        _obstacleView.Apply(new RunningGameObstacleView.Entity(speedCoefficient, spawnCoefficient));
    }

    public void Dispose()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }

        _disposables.Clear();
    }
}
