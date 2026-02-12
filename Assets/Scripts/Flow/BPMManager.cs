using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// BPMに基づいてビートイベントを発行するクラス
/// </summary>
public class BPMManager
{
    private readonly int totalBeats;
    private readonly float beatDuration;
    private readonly VoidEventChannelSO startBeatsEvent;
    private readonly IntEventChannelSO changeBeatsEvent;
    private readonly VoidEventChannelSO endBeatsEvent;

    private int currentBeatIndex = 0;
    private bool isPlaying = false;

    public BPMManager(int bpm,
        int totalBeats,
        VoidEventChannelSO startBeatsEvent = null,
        IntEventChannelSO changeBeatsEvent = null,
        VoidEventChannelSO endBeatsEvent = null)
    {
        this.beatDuration = 60f / bpm;
        this.totalBeats = totalBeats;
        this.startBeatsEvent = startBeatsEvent;
        this.changeBeatsEvent = changeBeatsEvent;
        this.endBeatsEvent = endBeatsEvent;
    }

    /// <summary>
    /// 指定されたビートカウントになるまで待機します
    /// </summary>
    public async UniTask AwaitCountUntilCount(int targetBeatIndex, CancellationToken token)
    {
        StartBeatsLoop(token).Forget();

        await UniTask.WaitUntil(() => currentBeatIndex == targetBeatIndex, cancellationToken: token);
    }

    /// <summary>
    /// ビートカウントを開始し、完了するまで待機します
    /// </summary>
    public async UniTask AwaitCount(CancellationToken token)
    {
        await StartBeatsLoop(token);
    }

    private async UniTask StartBeatsLoop(CancellationToken token)
    {
        if (isPlaying) return;
        isPlaying = true;

        currentBeatIndex = 0;

        double startTime = Time.unscaledTimeAsDouble;

        startBeatsEvent?.Raise();

        try
        {
            for (int i = 0; i < totalBeats; i++)
            {
                changeBeatsEvent?.Raise(currentBeatIndex);
                currentBeatIndex++;

                double nextBeatTime = startTime + ((i + 1) * beatDuration);
                float waitTime = (float)(nextBeatTime - Time.unscaledTimeAsDouble);
                waitTime = Mathf.Max(0f, waitTime);

                await UniTask.Delay(TimeSpan.FromSeconds(waitTime), ignoreTimeScale: true, cancellationToken: token);
            }

            endBeatsEvent?.Raise();
        }
        catch (OperationCanceledException)
        {
            // Cancelled
            throw;
        }
        finally
        {
            isPlaying = false;
        }
    }
}
