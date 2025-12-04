using System.Collections.Generic;
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
    private int score = 0;


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
            score += 100 + (int)(100 * 0.1 * continuousWinCount);
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

    public int GetCurrentScore()
    {
        return score;
    }
}
