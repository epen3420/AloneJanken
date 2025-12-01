using UnityEngine;
using UnityEngine.Events;

public abstract class EventChannelSO<T> : ScriptableObject
{
    public event UnityAction<T> OnRaised;


    public void Raise(T value)
    {
        OnRaised?.Invoke(value);
    }
}
