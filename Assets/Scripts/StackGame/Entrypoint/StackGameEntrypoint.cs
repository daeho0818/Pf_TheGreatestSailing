using UnityEngine;

public class StackGameEntrypoint : MonoBehaviour
{
    private ServiceLocator _serviceLocator;
    private StackGame _stackGame;

    private void Awake()
    {
        _serviceLocator = new ServiceLocator();
        _stackGame = new StackGame(_serviceLocator);
    }

    private void OnDestroy()
    {
        _stackGame.Dispose();
        _serviceLocator.Dispose();
    }
}
