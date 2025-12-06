using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEventChannelSO", menuName = "Events/VoidEventChannelSO")]
public class VoidEventChannelSO : ScriptableObject
{
    public event UnityAction OnVoidRaised;


    public void Raise()
    {
        if (OnVoidRaised == null)
        {
            Debug.LogWarning($"{name} was raised but no listeners were found.");
            return;
        }

        OnVoidRaised.Invoke();
    }


    public async UniTask WaitAsync(CancellationToken ctn = default)
    {
        var utcs = new UniTaskCompletionSource();

        UnityAction listener = () =>
        {
            utcs.TrySetResult();
        };

        OnVoidRaised += listener;

        try
        {
            await utcs.Task.AttachExternalCancellation(ctn);
        }
        finally
        {
            OnVoidRaised -= listener;
        }
    }
}
