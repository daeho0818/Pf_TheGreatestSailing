using UnityEngine;

public class RunningGameBackgroundViews : MonoBehaviour
{
    public readonly struct Entity
    {
        public float SpeedCoefficient { get; }

        public Entity(float speedCoefficient)
        {
            SpeedCoefficient = speedCoefficient;
        }
    }

    [SerializeField]
    private float _speedCoefficient;
    [SerializeField]
    private SpriteRenderer _current;
    [SerializeField]
    private SpriteRenderer _next;

    [SerializeField]
    private Transform _source;
    [SerializeField]
    private Transform _destination;

    private float _systemSpeedCoefficient = 1;

    public void Apply(Entity entity)
    {
        _systemSpeedCoefficient = entity.SpeedCoefficient;
    }

    private void Update()
    {
        if (_current.transform.position.x < _destination.position.x - _current.sprite.bounds.size.x)
        {
            _current.transform.position = _source.position;
            var temp = _current;
            _current = _next;
            _next = temp;
        }

        _current.transform.Translate((_destination.position - _source.position).normalized * _systemSpeedCoefficient * _speedCoefficient * Time.deltaTime, Space.World);
        _next.transform.position = _current.transform.position + _current.sprite.bounds.size.x * Vector3.right;
    }
}
