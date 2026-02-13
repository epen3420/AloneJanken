
using UnityEngine;

public class ScoreCalculator
{
    private readonly int baseScore;
    private readonly float continuousMultiplier;
    private readonly int maxAddContinuous;

    public ScoreCalculator(int baseScore = 100, float continuousMultiplier = 0.1f, int maxAddContinuous = 10)
    {
        this.baseScore = baseScore;
        this.continuousMultiplier = continuousMultiplier;
        this.maxAddContinuous = maxAddContinuous;
    }

    public int CalculateAddScore(int currentContinuousCount)
    {
        int clampedContinuousCount = Mathf.Clamp(currentContinuousCount, 0, maxAddContinuous);
        return baseScore + (int)(baseScore * continuousMultiplier * clampedContinuousCount);
    }
}
