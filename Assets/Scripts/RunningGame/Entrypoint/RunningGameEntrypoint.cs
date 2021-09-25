using UnityEngine;

public class RunningGameEntrypoint : MonoBehaviour
{
    private ServiceLocator _runningGameServiceLocator;
    private RunningGame _runningGame;

    private void Awake()
    {
        _runningGameServiceLocator = new ServiceLocator();
        _runningGame = new RunningGame(_runningGameServiceLocator);
    }

    private void OnDestroy()
    {
        _runningGame.Dispose();
        _runningGameServiceLocator.Dispose();
    }
}
