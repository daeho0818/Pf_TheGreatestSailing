using System;

public class StackGameInputPresenter : IDisposable
{
    private readonly StackGameInputView _view;

    public StackGameInputPresenter(ServiceLocator serviceLocator)
    {
        _view = serviceLocator.GetFromSceneObject<StackGameInputView>();
        _view.OnMouseClick += OnMouseClick;
    }

    private void OnMouseClick()
    {
        MessageBroker.Default.Publish(new StackGameMessages.InputMessage());
    }

    public void Dispose()
    {
        _view.OnMouseClick -= OnMouseClick;
    }
}
