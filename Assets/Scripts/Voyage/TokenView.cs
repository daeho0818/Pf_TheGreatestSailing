using System.Linq;
using UnityEngine;

public class TokenView : MonoBehaviour
{
    public readonly struct Entity
    {
        public int CurrentTokenCount { get; }

        public Entity(int currentTokenCount)
        {
            CurrentTokenCount = currentTokenCount;
        }
    }

    [SerializeField]
    private Transform _placeholder;
    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private float _space;
    [SerializeField]
    private GameObject _tokenPrefab;

    public void Apply(Entity entity)
    {
        foreach (var child in _placeholder.GetComponentsInChildren<Transform>().Skip(1))
        {
            GameObject.Destroy(child.gameObject);
        }

        var wholeSize = (_renderer.bounds.size.x * entity.CurrentTokenCount) + (_space * (entity.CurrentTokenCount - 1));
        foreach (var nextPosition in Enumerable.Range(0, entity.CurrentTokenCount).Select(x =>
        {
            var nextPosition = -wholeSize / 2 + x * _renderer.bounds.size.x;
            if (x > 0) nextPosition += _space * x;
            return nextPosition;
        }))
        {
            var token = GameObject.Instantiate(_tokenPrefab, _placeholder, instantiateInWorldSpace: false) as GameObject;
            token.transform.localPosition = new Vector3(nextPosition, token.transform.localPosition.y, token.transform.localPosition.z);
        }
    }
}
