
using NUnit.Framework;

public class ScoreCalculatorTest
{
    [Test]
    public void CalculateAddScore_NewGame_ReturnsBaseScore()
    {
        var calculator = new ScoreCalculator(100, 0.1f, 10);
        int score = calculator.CalculateAddScore(0);
        Assert.AreEqual(100, score);
    }

    [Test]
    public void CalculateAddScore_OneContinuousWin_ReturnsScoreWithBonus()
    {
        var calculator = new ScoreCalculator(100, 0.1f, 10);
        int score = calculator.CalculateAddScore(1);
        // 100 + (100 * 0.1 * 1) = 110
        Assert.AreEqual(110, score);
    }

    [Test]
    public void CalculateAddScore_MaxContinuousWin_ReturnsCappedBonus()
    {
        var calculator = new ScoreCalculator(100, 0.1f, 5); // Max 5
        int score = calculator.CalculateAddScore(10); // Current 10 (over max)

        // 100 + (100 * 0.1 * 5) = 150. Bonus capped at 5.
        Assert.AreEqual(150, score);
    }
}
