using System.Threading.Tasks;
using UnityEngine;

public class RunningGameObstacleView : MonoBehaviour
{
    public readonly struct Entity
    {
        public float SpeedCoefficient { get; }
        public float SpawnCoefficient { get; }

        public Entity(float speedCoefficient, float spawnCoefficient)
        {
            SpeedCoefficient = speedCoefficient;
            SpawnCoefficient = spawnCoefficient;
        }
    }

    [SerializeField]
    private RunningGameObstacleElementView[] _propsPrefab;

    [SerializeField, Range(0, 5000)]
    private int _initialDelayMilliseconds;
    [SerializeField, Range(0, 5000)]
    private int _minSpawnMilliseconds;
    [SerializeField, Range(0, 5000)]
    private int _maxSpawnMilliseconds;

    [SerializeField]
    private Transform[] _sourcePositions;
    [SerializeField]
    private Transform _destination;

    [SerializeField]
    private float _speedCoefficient;

    private bool _available = true;

    private int _nextPropsIndex;
    private float _systemSpeedCoefficient = 1;
    private float _spawnSpeedCoefficient = 1;

    private void Awake()
    {
        ObjectPool<RunningGameObstacleElementView>.Initialize(_propsPrefab, props => props[Random.Range(0, _nextPropsIndex++ % props.Count + 1)], 30);
        ObjectPool<RunningGameObstacleElementView>.Instance.Preload(30);

        _ = SpawnAsync();
    }

    public void Apply(Entity entity)
    {
        _systemSpeedCoefficient = entity.SpeedCoefficient;
        _spawnSpeedCoefficient = entity.SpawnCoefficient;
    }

    private async Task SpawnAsync()
    {
        await Task.Delay(_initialDelayMilliseconds);

        while (_available)
        {
            if (Mathf.Approximately(_systemSpeedCoefficient, 0)) await Task.Yield();

            var propView = ObjectPool<RunningGameObstacleElementView>.Instance.Rent();

            propView.gameObject.SetActive(true);
            propView.transform.position = _sourcePositions[(int)propView.State].position;

            _ = TranslateAndReturnAsync(propView);

            var delayMilliseconds = Random.Range(_minSpawnMilliseconds, _maxSpawnMilliseconds);

            await Task.Delay((int)(delayMilliseconds * _spawnSpeedCoefficient));
        }
    }

    private async Task TranslateAndReturnAsync(RunningGameObstacleElementView propView)
    {
        float direction;
        while (_available && (direction = _destination.position.x - propView.transform.position.x) < 0)
        {
            propView.transform.Translate(Vector3.right * Mathf.Sign(direction) * _speedCoefficient * _systemSpeedCoefficient * Time.deltaTime, Space.World);
            await Task.Yield();
        }

        ObjectPool<RunningGameObstacleElementView>.Instance?.Return(propView);
    }

    private void OnDestroy()
    {
        _available = false;
        ObjectPool<RunningGameObstacleElementView>.Instance.Dispose();
    }
}
