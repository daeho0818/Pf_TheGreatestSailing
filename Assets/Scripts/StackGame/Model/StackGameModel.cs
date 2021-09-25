using System;
using UnityEngine;

public class StackGameModel : ILifeGameModel, IDisposable
{
    public IReadOnlyReactiveProperty<StackGameState> GameState => _gameState;
    private readonly ReactiveProperty<StackGameState> _gameState;

    public IReadOnlyReactiveProperty<int> CurrentScore => _currentScore;
    private readonly ReactiveProperty<int> _currentScore;

    public IReadOnlyReactiveProperty<int> Life => _life;
    private readonly ReactiveProperty<int> _life;

    public StackGameData GameData { get; }

    public StackGameModel()
    {
        GameData = Resources.Load<StackGameData>("StackGame/Data/Stack Game Data");

        _gameState = new ReactiveProperty<StackGameState>();
        _currentScore = new ReactiveProperty<int>();
        _life = new ReactiveProperty<int>(GameData.InitialLife);
    }

    public void ChangeState(StackGameState state)
    {
        _gameState.Value = state;
    }

    public void Score()
    {
        _currentScore.Value += 1;

        if (_currentScore.Value >= GameData.SuccessScore)
        {
            GameMgr.Instance.GameClear(Enum.TokenType.Eta);
        }
        else
        {
            _gameState.Value = StackGameState.Spawn;
        }
    }

    public void Failed()
    {
        _life.Value -= 1;
        if (_life.Value <= 0)
        {
            GameMgr.Instance.GameFail(Enum.TokenType.Eta);
        }
        else
        {
            _currentScore.Value = 0;
            _gameState.Value = StackGameState.Spawn;
        }
    }

    public void Dispose()
    {

    }
}
