using System;

public class StackGame : IDisposable
{
    private readonly StackGameSpawnPresenter _spawnPresenter;
    private readonly StackGameInputPresenter _inputPresenter;
    private readonly StackGameJudgePresenter _judgePresenter;
    private readonly RunningGameLifeUIPresenter _lifeUIPresenter;
    private readonly StackGameScoreUIPresenter _stackGameScoreUIPresenter;
    private readonly StackGameModel _gameModel;

    public StackGame(ServiceLocator serviceLocator)
    {
        _gameModel = new StackGameModel();
        _spawnPresenter = new StackGameSpawnPresenter(serviceLocator, _gameModel);
        _inputPresenter = new StackGameInputPresenter(serviceLocator);
        _judgePresenter = new StackGameJudgePresenter(serviceLocator, _gameModel);
        _lifeUIPresenter = new RunningGameLifeUIPresenter(serviceLocator, _gameModel);
        _stackGameScoreUIPresenter = new StackGameScoreUIPresenter(serviceLocator, _gameModel);
    }

    public void Dispose()
    {
        _spawnPresenter.Dispose();
        _inputPresenter.Dispose();
        _judgePresenter.Dispose();
        _lifeUIPresenter.Dispose();
        _stackGameScoreUIPresenter.Dispose();
    }
}
