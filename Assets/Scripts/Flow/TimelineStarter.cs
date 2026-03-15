using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TimelineStarter : MonoBehaviour
{
    [SerializeField]
    private TimelineManager timelineManager;
    private CancellationTokenSource cts;

    private async void Start()
    {
        timelineManager.EndJanken += StartTimeline;
        StartTimeline();
    }

    private void StartTimeline()
    {
        Reset();
        cts = new CancellationTokenSource();

        timelineManager.Execute(cts.Token, true).Forget();
    }

    private void Reset()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = null;
    }

    private void OnDestroy()
    {
        Reset();
        timelineManager.EndJanken -= StartTimeline;
    }
}
