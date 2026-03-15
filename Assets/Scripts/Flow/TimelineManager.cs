using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public event UnityAction EndInput;
    public event UnityAction EndJanken;

    [SerializeField]
    private BoolEventChannelSO changeMouthEvent;

    private PlayableDirector director;


    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    public async UniTask Execute(CancellationToken ctn, bool isMute = false)
    {
        Mute(isMute);

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
        catch (System.OperationCanceledException)
        {
            director.Stop();

            throw;
        }
        finally
        {
            director.stopped -= listener;
        }
    }

    public void RaiseOpenMouth()
    {
        changeMouthEvent?.Raise(true);
    }

    public void RaiseCloseMouth()
    {
        changeMouthEvent?.Raise(false);
    }

    public void RaiseEndInput()
    {
        EndInput?.Invoke();
    }

    public void RaiseEndJanken()
    {
        EndJanken?.Invoke();
    }

    private void Mute(bool isMute)
    {
        var timelineAsset = director.playableAsset as TimelineAsset;
        if (timelineAsset == null) return;

        foreach (var track in timelineAsset.GetOutputTracks())
        {
            if (track is AudioTrack)
            {
                track.muted = isMute;
            }
        }

        var time = director.time;
        director.RebuildGraph();
        director.time = time;
    }
}
