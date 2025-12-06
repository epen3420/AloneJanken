using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public abstract class EventChannelSO<T> : VoidEventChannelSO
{
    public event UnityAction<T> OnRaised;


    public void Raise(T value)
    {
        base.Raise();

        if (OnRaised == null)
        {
            Debug.LogWarning($"{name} was raised but no listeners were found.");
            return;
        }

        OnRaised.Invoke(value);


#if UNITY_EDITOR
        Debug.Log($"{name} raised with value: {value}");
#endif
    }

    public async UniTask<T> WaitAsyncWithValue(CancellationToken ctn = default)
    {
        var utcs = new UniTaskCompletionSource<T>();

        UnityAction<T> listener = (val) =>
        {
            utcs.TrySetResult(val);
        };

        OnRaised += listener;

        try
        {
            return await utcs.Task.AttachExternalCancellation(ctn);
        }
        finally
        {
            OnRaised -= listener;
        }
    }
}
