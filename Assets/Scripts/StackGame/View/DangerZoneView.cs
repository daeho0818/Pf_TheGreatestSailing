using System;
using UnityEngine;

public class DangerZoneView : MonoBehaviour
{
    public readonly struct Entity
    {
        public bool Judged { get; }

        public Entity(bool judged)
        {
            Judged = judged;
        }
    }

    public event Action OnBoxCollide = () => { };

    [SerializeField]
    private DangerZoneElementView[] _elements;

    private bool _judged;

    private void Awake()
    {
        foreach (var element in _elements)
        {
            element.OnBoxCollide += () =>
            {
                if (!_judged)
                {
                    _judged = true;
                    OnBoxCollide.Invoke();
                }
            };
        }
    }

    public void Apply(Entity entity)
    {
        _judged = entity.Judged;
    }
}
