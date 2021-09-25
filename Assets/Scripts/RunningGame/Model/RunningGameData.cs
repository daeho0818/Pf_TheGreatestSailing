using UnityEngine;

[CreateAssetMenu]
public class RunningGameData : ScriptableObject
{
    [field: SerializeField]
    public int InitialLife { get; private set; }

    [field: SerializeField]
    public float VictorySeconds { get; private set; }

    [field: SerializeField]
    public RunningGameStageData[] Stages { get; private set; }
}

[System.Serializable]
public class RunningGameStageData
{
    [field: SerializeField]
    public float StageAppliedSecondsBefore { get; private set; }

    [field: SerializeField]
    public float SpeedCoefficient { get; private set; }
    [field: SerializeField]
    public float SpawnCoefficient { get; private set; }

    [field: SerializeField]
    public RunningGameObstaclePositionState[] AppearStates { get; private set; }
}
