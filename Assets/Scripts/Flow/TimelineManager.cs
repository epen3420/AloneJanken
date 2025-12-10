using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    private PlayableDirector director;


    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    public async UniTask Execute(CancellationToken ctn)
    {
        director.Play();

        var utcs = new UniTaskCompletionSource<PlayableDirector>();

        System.Action<PlayableDirector> listener = (val) =>
        {
            utcs.TrySetResult(val);
        };

        director.stopped += listener;

        try
        {
            await utcs.Task.AttachExternalCancellation(ctn);
        }
        finally
        {
            director.stopped -= listener;
        }
    }
}
