using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [SerializeField]
    private BoolEventChannelSO endJanken;
    [SerializeField]
    private ReactiveProperty<int> winCount;
    [SerializeField]
    private ReactiveProperty<int> continuousWinCount;
    [SerializeField]
    private ReactiveProperty<int> score;
    [SerializeField]
    private int baseScore = 100;
    [SerializeField]
    private float continuousMultiplier = 0.1f;
    [SerializeField]
    private int maxAddContinuous = 10;


    private void OnEnable()
    {
        endJanken.OnRaised += UpdateScore;
    }

    private void OnDisable()
    {
        endJanken.OnRaised -= UpdateScore;
    }

    private void UpdateScore(bool isWin)
    {
        if (isWin)
        {
            int clampedContinuousCount = Mathf.Clamp(continuousWinCount.Value, 0, maxAddContinuous);
            score.Value += baseScore + (int)(baseScore * continuousMultiplier * clampedContinuousCount);
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

    public int GetCurrentContinuous()
    {
        return continuousWinCount.Value;
    }
}
