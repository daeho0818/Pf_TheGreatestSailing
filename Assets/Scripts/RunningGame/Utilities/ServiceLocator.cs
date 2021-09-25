using System;
using System.Collections.Generic;
using System.Linq;

public class ServiceLocator : IDisposable
{
    private readonly Dictionary<System.Type, object> _caches = new Dictionary<System.Type, object>();

    public T Get<T>() where T : class, new()
    {
        return GetOrCreateCache(() => new T());
    }

    public T GetFromSceneObject<T>() where T : UnityEngine.Object
    {
        return GetOrCreateCache(() => UnityEngine.Object.FindObjectOfType<T>());
    }

    private T GetOrCreateCache<T>(Func<T> factory) where T : class
    {
        var type = typeof(T);
        T cache;
        if (_caches.TryGetValue(type, out var rawCache))
        {
            cache = rawCache as T;
        }
        else
        {
            rawCache = cache = factory.Invoke();
        }

        _caches[type] = rawCache;
        return cache;
    }

    public void Dispose()
    {
        foreach (var disposable in _caches.OfType<IDisposable>())
        {
            disposable.Dispose();
        }

        _caches.Clear();
    }
}
