using System;

public class RunningGame : IDisposable
{
    private readonly ServiceLocator _serviceLocator;

    private readonly RunningGameModel _runningGameModel;

    private readonly RunningGameInputPresenter _inputPresenter;
    private readonly RunningGamePlayerPresenter _playerPresenter;
    private readonly RunningGameLifeUIPresenter _lifeUIPresenter;
    private readonly RunningGameBackgroundPresenter _backgroundPresenter;
    private readonly RunningGameSpeedPresenter _speedPresenter;
    private readonly RunningGameTimeUIPresenter _timeUIPresenter;

    public RunningGame(ServiceLocator serviceLocator)
    {
        _serviceLocator = serviceLocator;
        _runningGameModel = new RunningGameModel();
        _inputPresenter = new RunningGameInputPresenter(_serviceLocator);
        _playerPresenter = new RunningGamePlayerPresenter(_serviceLocator, _runningGameModel);
        _lifeUIPresenter = new RunningGameLifeUIPresenter(_serviceLocator, _runningGameModel);
        _backgroundPresenter = new RunningGameBackgroundPresenter(_serviceLocator, _runningGameModel);
        _speedPresenter = new RunningGameSpeedPresenter(_serviceLocator, _runningGameModel);
        _timeUIPresenter = new RunningGameTimeUIPresenter(_serviceLocator, _runningGameModel);
    }

    public void Dispose()
    {
        _runningGameModel.Dispose();
        _inputPresenter.Dispose();
        _playerPresenter.Dispose();
        _lifeUIPresenter.Dispose();
        _backgroundPresenter.Dispose();
        _speedPresenter.Dispose();
        _timeUIPresenter.Dispose();
    }
}
