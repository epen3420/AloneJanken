using UnityEngine;
using UnityEngine.Events;

public abstract class EventChannelSO<T> : VoidEventChannelSO
{
    public event UnityAction<T> OnRaised;


    public void Raise(T value)
    {
        base.Raise();
        OnRaised?.Invoke(value);
    }
}
