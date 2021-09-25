using System;
using System.Threading.Tasks;

public class StackGameJudgePresenter : IDisposable
{
    private readonly StackGameModel _model;
    private readonly SpawnBoxView _view;
    private readonly DangerZoneView _dangerZoneView;

    private float _previousBoxY = -10;
    private bool _firstTry = true;

    public StackGameJudgePresenter(ServiceLocator serviceLocator, StackGameModel model)
    {
        _model = model;
        _model.Life.OnValueChanged += OnFailed;

        _view = serviceLocator.GetFromSceneObject<SpawnBoxView>();
        _view.OnBoxStopped += OnBoxStopped;

        _dangerZoneView = serviceLocator.GetFromSceneObject<DangerZoneView>();
        _dangerZoneView.OnBoxCollide += () => _ = OnBoxCollideAsync();
    }

    private async Task OnBoxCollideAsync()
    {
        if (_firstTry)
        {
            _firstTry = false;
            _dangerZoneView.Apply(new DangerZoneView.Entity(judged: false));
        }
        else
        {
            await Task.Delay(1000);

            _model.Failed();
        }
    }

    private void OnFailed(int _)
    {
        _previousBoxY = -10;
        _firstTry = true;
        _dangerZoneView.Apply(new DangerZoneView.Entity(judged: false));
    }

    private void OnBoxStopped(UnityEngine.Vector3 position)
    {
        _model.Score();
        _previousBoxY = position.y;
    }

    public void Dispose() { }
}
