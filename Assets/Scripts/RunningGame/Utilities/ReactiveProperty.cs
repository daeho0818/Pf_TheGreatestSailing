using System;

public interface IReadOnlyReactiveProperty<T>
{
    public event Action<T> OnValueChanged;
    public T Value { get; }
}

public class ReactiveProperty<T> : IReadOnlyReactiveProperty<T>
{
    public event Action<T> OnValueChanged = _ => { };

    private readonly bool _notifyOnSameValue;

    private T _value;
    public T Value
    {
        get => _value;
        set
        {
            var valueIsDifferent = !_value.Equals(value);
            _value = value;

            if (valueIsDifferent || _notifyOnSameValue)
            {
                OnValueChanged.Invoke(value);
            }
        }
    }

    public ReactiveProperty(T initialValue = default, bool notifyOnSameValue = true)
    {
        _notifyOnSameValue = notifyOnSameValue;
        Value = initialValue;
    }
}
