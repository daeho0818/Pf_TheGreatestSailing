using System;
using System.Collections.Generic;

public class MessageBroker
{
    public static readonly MessageBroker Default = new MessageBroker();

    private readonly Dictionary<Type, List<object>> _subscriptions = new Dictionary<Type, List<object>>();

    public void Publish<T>(T message)
    {
        if (_subscriptions.TryGetValue(typeof(T), out var subscriptions))
        {
            foreach (var subscription in subscriptions)
            {
                (subscription as Action<T>).Invoke(message);
            }
        }
    }

    public IDisposable Receive<T>(Action<T> onReceive)
    {
        var type = typeof(T);
        if (!_subscriptions.TryGetValue(typeof(T), out var subscriptions))
        {
            _subscriptions[type] = subscriptions = new List<object>();
        }

        subscriptions.Add(onReceive);

        return new AnonymousDisposable(() => subscriptions.Remove(onReceive));
    }
}

public class AnonymousDisposable : IDisposable
{
    private bool _isDisposed = false;
    private Action _onDispose;

    public AnonymousDisposable(Action onDispose)
    {
        _onDispose = onDispose;
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _isDisposed = true;
            _onDispose();
            _onDispose = null;
        }
    }
}

public static class DisposableExtension
{
    public static void AddTo(this IDisposable disposable, List<IDisposable> disposables)
    {
        disposables.Add(disposable);
    }
}
