using System;
using System.Collections.Generic;

public class RunningGameSpeedPresenter : IDisposable
{
    private readonly RunningGameModel _model;

    private readonly List<IDisposable> _disposables = new List<IDisposable>();

    private float _stageSpawnCoefficient = 1;
    private float _stageSpeedCoefficient = 1;
    private float _systemSpeedCoefficient = 1;

    public RunningGameSpeedPresenter(ServiceLocator _serviceLocator, RunningGameModel model)
    {
        _model = model;
        _model.CurrentStageIndex.OnValueChanged += OnStageIndexChanged;

        MessageBroker.Default.Receive<RunningGameMessages.SystemSpeedCoefficientChangedMessage>(x =>
            {
                _systemSpeedCoefficient = x.SystemSpeedCoefficient;
                PublishSpeed();
            })
            .AddTo(_disposables);
    }

    private void PublishSpeed()
    {
        MessageBroker.Default.Publish(new RunningGameMessages.CoefficientChangedMessage(_stageSpeedCoefficient * _systemSpeedCoefficient, _stageSpawnCoefficient));
    }

    private void OnStageIndexChanged(int _)
    {
        _stageSpeedCoefficient = _model.CurrentStage.SpeedCoefficient;
        PublishSpeed();
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
