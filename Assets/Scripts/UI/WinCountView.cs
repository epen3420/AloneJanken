using TMPro;
using UnityEngine;

public class WinCountView : MonoBehaviour
{
    [SerializeField]
    private IntEventChannelSO changeWinCount;
    [SerializeField]
    private IntEventChannelSO changeContinuousWinCount;
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text winCountText;

    private ScoreManager scoreManager;


    private void Start()
    {
        scoreManager = ScoreManager.Instance;

        SetScoreText();
        SetWinCountText();
    }

    private void OnEnable()
    {
        changeWinCount.OnVoidRaised += SetScoreText;
        changeContinuousWinCount.OnVoidRaised += SetWinCountText;
    }

    private void OnDisable()
    {
        changeWinCount.OnVoidRaised += SetScoreText;
        changeContinuousWinCount.OnVoidRaised -= SetWinCountText;
    }

    private void SetScoreText()
    {
        if (scoreManager == null)
            scoreManager = ScoreManager.Instance;

        scoreText.SetText($"Score {scoreManager.GetCurrentScore()}");
    }

    private void SetWinCountText()
    {
        if (scoreManager == null)
            scoreManager = ScoreManager.Instance;

        winCountText.SetText($"連続成功 {scoreManager.GetCurrentContinuous()}回");
    }
}
