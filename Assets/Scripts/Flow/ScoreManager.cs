using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private BoolEventChannelSO endRound;
    [SerializeField]
    private ReactiveProperty<int> winCount;
    [SerializeField]
    private ReactiveProperty<int> continuousWinCount;
    [SerializeField]
    private ReactiveProperty<int> score;


    private void OnEnable()
    {
        endRound.OnRaised += UpdateScore;
    }

    private void OnDisable()
    {
        endRound.OnRaised -= UpdateScore;
    }

    private void UpdateScore(bool isWin)
    {
        if (isWin)
        {
            score.Value += 100 + (int)(100 * 0.1 * continuousWinCount.Value);
            winCount.Value++;
            continuousWinCount.Value++;
        }
        else
        {
            continuousWinCount.Value = 0;
        }
    }

    public int GetCurrentScore()
    {
        return score.Value;
    }
}
