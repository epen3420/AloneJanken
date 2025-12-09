using System.Threading;
using Cysharp.Threading.Tasks;
using Mathf = UnityEngine.Mathf;
using Time = UnityEngine.Time;

public class BPMManager
{
    private readonly int beatsNum = 8;
    private readonly float duration;
    private readonly VoidEventChannelSO startBeats;
    private readonly IntEventChannelSO changeBeats;
    private readonly VoidEventChannelSO endBeats;

    private int currentBeats = 0;
    private bool isPlaying = false;

    public BPMManager(int bpm,
        int beatsNum,
        VoidEventChannelSO startBeats = null,
        IntEventChannelSO changeBeats = null,
        VoidEventChannelSO endBeats = null)
    {
        duration = 60f / bpm;
        this.beatsNum = beatsNum;
        this.startBeats = startBeats;
        this.changeBeats = changeBeats;
        this.endBeats = endBeats;
    }

    public async UniTask AwaitCountUntilCount(int countNum, CancellationToken ctn)
    {
        AwaitCount(ctn).Forget();

        await UniTask.WaitUntil(() => currentBeats == countNum, cancellationToken: ctn);
    }

    public async UniTask AwaitCount(CancellationToken ctn)
    {
        if (isPlaying) return;
        isPlaying = true;

        currentBeats = 0;

        double startTime = Time.unscaledTimeAsDouble;

        startBeats?.Raise();
        for (int i = 0; i < beatsNum; i++)
        {
            changeBeats?.Raise(currentBeats);
            currentBeats++;

            double nextBeatTime = startTime + ((i + 1) * duration);

            float waitTime = (float)(nextBeatTime - Time.unscaledTimeAsDouble);

            waitTime = Mathf.Max(0f, waitTime);

            try
            {
                await UniTask.Delay(System.TimeSpan.FromSeconds(waitTime),
                                    ignoreTimeScale: false,
                                    cancellationToken: ctn);
            }
            catch (System.OperationCanceledException)
            {
                isPlaying = false;
                return;
            }
        }

        endBeats?.Raise();

        isPlaying = false;
    }
}
