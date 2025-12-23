using Cysharp.Threading.Tasks;
using SoundSystem;
using UnityEngine;

public class ResultPresenter : MonoBehaviour
{
    [SerializeField]
    private ResultViewer continuousViewer;
    [SerializeField]
    private ResultViewer resultViewer;

#if UNITY_EDITOR
    [SerializeField]
    private int score = 1234;
    [SerializeField]
    private int continuous = 12;
#endif

    private async void Start()
    {
#if UNITY_EDITOR
        if (ScoreManager.Instance == null)
        {
            continuousViewer.CountScore(continuous, 0.5f).Forget();

            SoundPlayer.Instance.PlayBgm("score_anim", ctn: destroyCancellationToken).Forget();
            await resultViewer.CountScore(score, 1);
            SoundPlayer.Instance.StopBgm();
            SoundPlayer.Instance.PlaySe("score");
            return;
        }
#endif
        continuousViewer.CountScore(ScoreManager.Instance.GetMaxContinuous(), 0.5f).Forget();

        SoundPlayer.Instance.PlayBgm("score_anim", ctn: destroyCancellationToken).Forget();
        await resultViewer.CountScore(ScoreManager.Instance.GetCurrentScore(), 1);
        SoundPlayer.Instance.StopBgm();
        SoundPlayer.Instance.PlaySe("score");
    }
}
