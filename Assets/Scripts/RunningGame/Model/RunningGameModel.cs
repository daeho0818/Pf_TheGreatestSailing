using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface ILifeGameModel
{
    IReadOnlyReactiveProperty<int> Life { get; }
}


public class RunningGameModel : ILifeGameModel, IDisposable
{
    public IReadOnlyReactiveProperty<int> Life => _life;
    private readonly ReactiveProperty<int> _life;

    public IReadOnlyReactiveProperty<float> Time => _time;
    private readonly ReactiveProperty<float> _time;

    public IReadOnlyReactiveProperty<RunningGamePlayerState> PlayerState => _playerState;
    private readonly ReactiveProperty<RunningGamePlayerState> _playerState;

    public RunningGameStageData CurrentStage => GameData.Stages[_currentStageIndex.Value];
    public IReadOnlyReactiveProperty<int> CurrentStageIndex => _currentStageIndex;
    private readonly ReactiveProperty<int> _currentStageIndex;

    private readonly CancellationTokenSource _cts = new CancellationTokenSource();

    public RunningGameData GameData { get; }

    public RunningGameModel()
    {
        GameData = Resources.Load<RunningGameData>("RunningGame/Data/Running Game Data");
        _currentStageIndex = new ReactiveProperty<int>();

        _life = new ReactiveProperty<int>(GameData.InitialLife);
        _life.OnValueChanged += OnLifeChanged;

        _time = new ReactiveProperty<float>();
        _time.OnValueChanged += OnTimeChanged;

        _playerState = new ReactiveProperty<RunningGamePlayerState>();

        _ = ProceedTimer();
    }

    private void OnLifeChanged(int currentLife)
    {
        if (currentLife <= 0)
        {
            GameMgr.Instance.GameFail(Enum.TokenType.Epsilon);
        }
    }

    private void OnTimeChanged(float currentMilliseconds)
    {
        var currentSeconds = currentMilliseconds / 1000;
        if (JudgeVictory(currentSeconds))
        {
            GameMgr.Instance.GameClear(Enum.TokenType.Epsilon);
        }

        if (currentMilliseconds >= CurrentStage.StageAppliedSecondsBefore)
        {
            _currentStageIndex.Value = GameData.Stages.Select((stage, index) => (stage, index))
                .Last(x => x.stage.StageAppliedSecondsBefore < currentSeconds).index;
        }
    }

    private bool JudgeVictory(float currentSeconds)
    {
        return GameData.VictorySeconds < currentSeconds;
    }

    private async Task ProceedTimer()
    {
        const int delayMilliseconds = 10;
        while (!_cts.IsCancellationRequested)
        {
            await Task.Delay(delayMilliseconds);
            _time.Value += delayMilliseconds;
        }
    }

    public void CheckDamaged(RunningGameObstaclePositionState gameObstaclePositionState)
    {
        bool damaged;
        switch (_playerState.Value)
        {
            case RunningGamePlayerState.Normal
            when gameObstaclePositionState is RunningGameObstaclePositionState.Bottom ||
                gameObstaclePositionState is RunningGameObstaclePositionState.Middle:
                damaged = true;
                break;

            case RunningGamePlayerState.Jumping
            when gameObstaclePositionState is RunningGameObstaclePositionState.Top ||
                gameObstaclePositionState is RunningGameObstaclePositionState.Middle:
                damaged = true;
                break;

            case RunningGamePlayerState.Ducking when gameObstaclePositionState is RunningGameObstaclePositionState.Bottom:
                damaged = true;
                break;

            default:
                damaged = false;
                break;
        }

        if (damaged)
        {
            _life.Value -= 1;
        }
    }

    public void ChangeState(RunningGamePlayerState state)
    {
        _playerState.Value = state;
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
