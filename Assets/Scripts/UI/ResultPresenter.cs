using Cysharp.Threading.Tasks;
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

    private void Start()
    {
#if UNITY_EDITOR
        if (ScoreManager.Instance == null)
        {
            resultViewer.CountScore(score, 1f).Forget();
            continuousViewer.CountScore(continuous, 0.5f).Forget();
            return;
        }
#endif
        continuousViewer.CountScore(ScoreManager.Instance.GetMaxContinuous(), 0.5f).Forget();

        resultViewer.CountScore(ScoreManager.Instance.GetCurrentScore(), 1).Forget();
    }
}
