using System;

public class RunningGameTimeUIPresenter : IDisposable
{
    private readonly ServiceLocator _serviceLocator;
    private readonly RunningGameModel _model;

    private readonly RunningGameTimeUIView _view;

    private bool _available = true;

    public RunningGameTimeUIPresenter(ServiceLocator serviceLocator, RunningGameModel gameModel)
    {
        _serviceLocator = serviceLocator;

        _model = gameModel;
        _model.Time.OnValueChanged += OnTimeChanged;
        _model.Life.OnValueChanged += OnLifeChanged;

        _view = ResourcesUtility.Instantiate<RunningGameTimeUIView>("RunningGame/Prefabs/UI/TimeUIView", UICanvas.Instance.transform);

        OnTimeChanged(_model.Time.Value);
        OnLifeChanged(_model.Life.Value);
    }

    private void OnLifeChanged(int currentLife)
    {
        _available = currentLife > 0;
    }

    private void OnTimeChanged(float timeMilliseconds)
    {
        if (_available)
        {
            _view.Apply(new RunningGameTimeUIView.Entity(timeMilliseconds / 1000));
        }
    }

    public void Dispose()
    {
        if (_view)
        {
            UnityEngine.Object.Destroy(_view.gameObject);
        }
    }
}
