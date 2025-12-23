using System.Collections.Generic;
using System.Linq;
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

    private List<int> continuousCounts = new List<int>();


    private void OnEnable()
    {
        endJanken.OnRaised += UpdateScore;
        SceneController.OnLoadedScene += ResetScore;
    }

    private void OnDisable()
    {
        endJanken.OnRaised -= UpdateScore;
        SceneController.OnLoadedScene -= ResetScore;
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
            continuousCounts.Add(continuousWinCount.Value);
            continuousWinCount.Value = 0;
        }
    }

    private void ResetScore(string sceneName)
    {
        if (sceneName == "Title")
        {
            winCount.Value = 0;
            continuousWinCount.Value = 0;
            score.Value = 0;
        }
    }

    public int GetCurrentWinCount()
    {
        return winCount.Value;
    }

    public int GetCurrentScore()
    {
        return score.Value;
    }

    public int GetCurrentContinuous()
    {
        return continuousWinCount.Value;
    }

    public int GetMaxContinuous()
    {
        return continuousCounts.Max();
    }
}
