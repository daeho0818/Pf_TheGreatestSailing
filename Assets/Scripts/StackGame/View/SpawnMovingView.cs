using UnityEngine;

public class SpawnMovingView : MonoBehaviour
{
    public readonly struct Entity
    {
        public Vector3 InitialPosition { get; }

        public Entity(Vector3 initialPosition)
        {
            InitialPosition = initialPosition;
        }
    }

    [SerializeField]
    private Transform _leftBoundary;
    [SerializeField]
    private Transform _rightBoundary;

    [SerializeField]
    private float _minSpeedCoefficient;
    [SerializeField]
    private float _maxSpeedCoefficient;

    [SerializeField]
    private BoxView _boxPrefab;

    public void Apply(Entity entity)
    {
        transform.position = entity.InitialPosition;
    }

    private Vector3 _direction = Vector3.right;

    private void Update()
    {
        transform.Translate(_direction * Random.Range(_minSpeedCoefficient, _maxSpeedCoefficient) * Time.deltaTime, Space.World);

        if (transform.position.x < _leftBoundary.position.x + _boxPrefab.ExtentsX)
        {
            _direction = Vector3.right;
        }
        else if (transform.position.x > _rightBoundary.position.x - _boxPrefab.ExtentsX)
        {
            _direction = Vector3.left;
        }
    }
}
