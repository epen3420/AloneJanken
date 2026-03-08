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


    private void Start()
    {
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
        scoreText.SetText($"Score {ScoreManager.Instance.GetCurrentScore()}");
    }

    private void SetWinCountText()
    {
        winCountText.SetText($"連続成功 {ScoreManager.Instance.GetCurrentContinuous()}回");
    }
}
