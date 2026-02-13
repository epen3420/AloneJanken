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
        int finalScore = 0;
        int maxContinuous = 0;

        if (ScoreManager.Instance != null)
        {
            finalScore = ScoreManager.Instance.GetCurrentScore();
            maxContinuous = ScoreManager.Instance.GetMaxContinuous();
        }
#if UNITY_EDITOR
        else
        {
            finalScore = score;
            maxContinuous = continuous;
        }
#endif

        if (continuousViewer != null)
        {
            continuousViewer.CountScore(maxContinuous, 0.5f).Forget();
        }

        if (SoundPlayer.Instance != null)
        {
            SoundPlayer.Instance.PlayBgm("score_anim", ctn: destroyCancellationToken).Forget();
        }

        if (resultViewer != null)
        {
            await resultViewer.CountScore(finalScore, 1f);
        }

        if (SoundPlayer.Instance != null)
        {
            SoundPlayer.Instance.StopBgm();
            SoundPlayer.Instance.PlaySe("score");
        }
    }
}
