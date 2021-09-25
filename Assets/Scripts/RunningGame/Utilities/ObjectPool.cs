using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool<T> : IDisposable where T : Component
{
    public static ObjectPool<T> Instance { get; private set; }

    private readonly Queue<T> _pool;
    private readonly Lazy<IReadOnlyList<T>> _prefabLazy;
    private readonly Func<IReadOnlyList<T>, T> _targetPredicator;

    private IReadOnlyList<T> _prefabs;

    public static void Initialize(string prefabPath, int capacity)
    {
        Instance = new ObjectPool<T>(prefabPath, capacity);
    }

    public static void Initialize(T prefab, int capacity)
    {
        Instance = new ObjectPool<T>(prefab, capacity);
    }

    public static void Initialize(IReadOnlyList<T> prefabs, Func<IReadOnlyList<T>, T> targetPredicator, int capacity)
    {
        Instance = new ObjectPool<T>(prefabs, targetPredicator, capacity);
    }

    public ObjectPool(string prefabPath, int capacity)
    {
        _pool = new Queue<T>(capacity);
        _prefabLazy = new Lazy<IReadOnlyList<T>>(() => _prefabs = new[] { ResourcesUtility.Load<T>(prefabPath) });
        _prefabs = Array.Empty<T>();
    }

    public ObjectPool(T prefab, int capacity)
    {
        _pool = new Queue<T>(capacity);
        _prefabs = new List<T>() { prefab };
        _targetPredicator = x => x.First();
    }

    public ObjectPool(IReadOnlyList<T> prefabs, Func<IReadOnlyList<T>, T> targetPredicator, int capacity)
    {
        _pool = new Queue<T>(capacity);
        _targetPredicator = targetPredicator;
        _prefabs = prefabs;
    }

    public void Preload(int count)
    {
        foreach (var _ in Enumerable.Repeat(0, count))
        {
            CreateAndEnqueueInstance();
        }
    }

    private T CreateAndEnqueueInstance()
    {
        var prefab = _prefabs.Any() ? _targetPredicator?.Invoke(_prefabs) : _prefabLazy.Value[0];
        var instance = UnityEngine.Object.Instantiate(prefab);
        instance.gameObject.SetActive(false);

        _pool.Enqueue(instance);

        return instance;
    }

    public T Rent()
    {
        if (_pool.Count <= 0)
        {
            CreateAndEnqueueInstance();
        }

        var instance = _pool.Dequeue();
        instance.gameObject.SetActive(true);

        return instance;
    }

    public void Return(T instance)
    {
        instance.gameObject.SetActive(false);
        _pool.Enqueue(instance);
    }

    public void Dispose()
    {
        foreach (var pool in _pool.Where(x => x))
        {
            UnityEngine.Object.Destroy(pool.gameObject);
        }

        _pool.Clear();

        Instance = default;
    }
}
