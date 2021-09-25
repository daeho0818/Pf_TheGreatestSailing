using System.Threading.Tasks;
using UnityEngine;

public class RunningGameBackgroundPropView : MonoBehaviour
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
    private RunningGameBackgroundPropElementView[] _propsPrefab;

    [SerializeField, Range(0, 5000)]
    private int _minSpawnMilliseconds;
    [SerializeField, Range(0, 5000)]
    private int _maxSpawnMilliseconds;

    [SerializeField]
    private Transform _source;
    [SerializeField]
    private Transform _destination;

    [SerializeField]
    private float _speedCoefficient;

    private bool _available = true;
    private bool _firstProp = true;

    private float _systemSpeedCoefficient = 1;
    private float _spawnCoefficient = 1;

    private void Awake()
    {
        ObjectPool<RunningGameBackgroundPropElementView>.Initialize(_propsPrefab, props => props[Random.Range(1, props.Count)], 30);
        ObjectPool<RunningGameBackgroundPropElementView>.Instance.Preload(30);

        _ = SpawnAsync();
    }

    public void Apply(Entity entity)
    {
        _systemSpeedCoefficient = entity.SpeedCoefficient;
        _spawnCoefficient = entity.SpawnCoefficient;
    }

    private async Task SpawnAsync()
    {
        while (_available)
        {
            if (Mathf.Approximately(_systemSpeedCoefficient, 0)) await Task.Yield();

            var propView = _firstProp ? GameObject.Instantiate(_propsPrefab[0]) : ObjectPool<RunningGameBackgroundPropElementView>.Instance.Rent();
            propView.gameObject.SetActive(true);
            propView.transform.position = _source.position;

            _ = TranslateAndReturnAsync(_firstProp, propView);

            _firstProp = false;

            var delayMilliseconds = _firstProp ? 1000 : Random.Range(_minSpawnMilliseconds, _maxSpawnMilliseconds);

            await Task.Delay((int)(delayMilliseconds * _spawnCoefficient));
        }
    }

    private async Task TranslateAndReturnAsync(bool firstProp, RunningGameBackgroundPropElementView propView)
    {
        float direction;
        while (_available && (direction = _destination.position.x - propView.transform.position.x) < 0)
        {
            propView.transform.Translate(Vector3.right * Mathf.Sign(direction) * _speedCoefficient * _systemSpeedCoefficient * Time.deltaTime, Space.World);
            await Task.Yield();
        }

        if (firstProp)
        {
            GameObject.Destroy(propView.gameObject);
        }
        else
        {
            ObjectPool<RunningGameBackgroundPropElementView>.Instance?.Return(propView);
        }
    }

    private void OnDestroy()
    {
        _available = false;
        ObjectPool<RunningGameBackgroundPropElementView>.Instance.Dispose();
    }
}
