using TMPro;
using UnityEngine;

public class WinCountView : MonoBehaviour
{
    [SerializeField]
    private BoolEventChannelSO endRound;
    [SerializeField]
    private TMP_Text winCountText;

    private int winCount = 0;
    private int continuousWinCount = 0;


    private void Start()
    {
        SetText(0);
    }

    private void OnEnable()
    {
        endRound.OnRaised += UpdateView;
    }

    private void OnDisable()
    {
        endRound.OnRaised -= UpdateView;
    }

    private void UpdateView(bool isWin)
    {
        if (isWin)
        {
            winCount++;
            continuousWinCount++;
        }
        else
        {
            continuousWinCount = 0;
        }

        SetText(winCount);
    }

    private void SetText(int count)
    {
        winCountText.SetText($"Win: {count}");
    }
}
