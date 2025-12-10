using TMPro;
using UnityEngine;

public class WinCountView : MonoBehaviour
{
    [SerializeField]
    private IntEventChannelSO changeWinCount;
    [SerializeField]
    private IntEventChannelSO changeContinuousWinCount;
    [SerializeField]
    private TMP_Text winCountText;


    private void Start()
    {
        SetText(0);
    }

    private void OnEnable()
    {
        changeWinCount.OnRaised += SetText;
    }

    private void OnDisable()
    {
        changeWinCount.OnRaised += SetText;
    }

    private void SetText(int count)
    {
        winCountText.SetText($"Score: {ScoreManager.Instance.GetCurrentScore()}");
    }
}
