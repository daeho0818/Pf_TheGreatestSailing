using System;
using UnityEngine;

public class RunningGameInputPresenter : IDisposable
{
    private readonly RunningGameInputView _view;

    public RunningGameInputPresenter(ServiceLocator _serviceLocator)
    {
        _view = _serviceLocator.GetFromSceneObject<RunningGameInputView>();
        _view.OnKeyDown += OnKeyDown;
        _view.OnKeyUp += OnKeyUp;
    }

    private void OnKeyDown(KeyCode keyCode)
    {
        MessageBroker.Default.Publish(new RunningGameMessages.KeyDownMessage(keyCode));
    }

    private void OnKeyUp(KeyCode keyCode)
    {
        MessageBroker.Default.Publish(new RunningGameMessages.KeyUpMessage(keyCode));
    }

    public void Dispose()
    {
        _view.OnKeyDown -= OnKeyDown;
    }
}
