using System;

public class StackGameScoreUIPresenter : IDisposable
{
    private readonly ServiceLocator _serviceLocator;
    private readonly StackGameModel _model;

    private readonly StackGameScoreUIView _view;

    public StackGameScoreUIPresenter(ServiceLocator serviceLocator, StackGameModel gameModel)
    {
        _serviceLocator = serviceLocator;

        _model = gameModel;
        _model.CurrentScore.OnValueChanged += OnScoreChanged;

        _view = ResourcesUtility.Instantiate<StackGameScoreUIView>("StackGame/Prefabs/ScoreUIView", UICanvas.Instance.transform);

        OnScoreChanged(_model.CurrentScore.Value);
    }

    private void OnScoreChanged(int score)
    {
        _view.Apply(new StackGameScoreUIView.Entity(score));
    }

    public void Dispose()
    {
        if (_view)
        {
            UnityEngine.Object.Destroy(_view.gameObject);
        }
    }
}
