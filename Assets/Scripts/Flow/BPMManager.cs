using System.Threading;
using Cysharp.Threading.Tasks;

public class BPMManager
{
    private int beatsNum = 8;
    private float duration;
    private int currentBeats = 0;
    private IntEventChannelSO changeBeats;

    public BPMManager(int bpm, int beatsNum, IntEventChannelSO changeBeats = null)
    {
        duration = 60f / bpm;
        this.beatsNum = beatsNum;
        this.changeBeats = changeBeats;
    }

    public async UniTask CountBeatsAsync(CancellationToken ctn)
    {
        currentBeats = 0;

        // changeBeat?.Raise(currentBeats++);

        for (int i = 0; i < beatsNum; i++)
        {
            try
            {
                await UniTask.Delay(System.TimeSpan.FromSeconds(duration),
                                    ignoreTimeScale: false,
                                    cancellationToken: ctn);

                changeBeats?.Raise(currentBeats++);
            }
            catch (System.OperationCanceledException)
            {
                // キャンセルされた場合はループを抜ける
                return;
            }
        }
    }
}
