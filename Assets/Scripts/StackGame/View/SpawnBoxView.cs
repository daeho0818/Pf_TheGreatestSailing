using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnBoxView : MonoBehaviour
{
    public readonly struct Entity
    {
        public StackGameState State { get; }

        public Entity(StackGameState state)
        {
            State = state;
        }
    }

    public event Action<Vector3> OnBoxStopped = boxPosition => { };

    [SerializeField]
    private SpawnMovingView _spawnMovingView;

    [SerializeField]
    private Transform[] _positionTransforms;

    [SerializeField]
    private BoxView _boxViewPrefab;

    private List<BoxView> _boxes = new List<BoxView>();

    public void Apply(Entity entity)
    {
        switch (entity.State)
        {
            case StackGameState.Spawn:
                var spawnTransform = _positionTransforms[UnityEngine.Random.Range(0, _positionTransforms.Length)];
                var currentBoxView = GameObject.Instantiate(_boxViewPrefab);
                currentBoxView.OnBoxStopped += () => OnBoxStopped.Invoke(currentBoxView.transform.position);
                currentBoxView.transform.SetParent(_spawnMovingView.transform, worldPositionStays: false);

                _boxes.Add(currentBoxView);
                _spawnMovingView.Apply(new SpawnMovingView.Entity(spawnTransform.position));

                break;

            case StackGameState.Drop:

                var boxView = _boxes.Last();
                boxView.transform.SetParent(null);
                boxView.Apply(new BoxView.Entity(applyGravity: true));

                break;
        }
    }

    public void Clean()
    {
        foreach (var box in _boxes)
        {
            Destroy(box.gameObject);
        }

        _boxes.Clear();
    }
}
