using System;

public class StackGameSpawnPresenter : IDisposable
{
    private readonly StackGameModel _model;
    private readonly SpawnBoxView _view;

    public StackGameSpawnPresenter(ServiceLocator serviceLocator, StackGameModel model)
    {
        _model = model;
        _model.GameState.OnValueChanged += OnGameStateChanged;
        _model.Life.OnValueChanged += OnFailed;

        _view = serviceLocator.GetFromSceneObject<SpawnBoxView>();

        OnGameStateChanged(_model.GameState.Value);

        MessageBroker.Default.Receive<StackGameMessages.InputMessage>(InputMessage);
    }

    private void OnFailed(int _)
    {
        _view.Clean();
    }

    private void OnGameStateChanged(StackGameState state)
    {
        _view.Apply(new SpawnBoxView.Entity(state));
    }

    private void InputMessage(StackGameMessages.InputMessage msg)
    {
        if (_model.GameState.Value == StackGameState.Spawn)
        {
            _model.ChangeState(StackGameState.Drop);
        }
    }

    public void Dispose() { }
}
