
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class HandJudgerTest
{
    [Test]
    public void Judge_DrawOne_ReturnsDrawOne()
    {
        var hands = new List<Hand>
        {
            new Hand(HandType.Rock, HandPosType.LeftUp),
            new Hand(HandType.Rock, HandPosType.RightUp),
        };

        var result = HandJudger.Judge(hands).ToList();

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(ResultType.DrawOne, result[0].Result);
        Assert.AreEqual(ResultType.DrawOne, result[1].Result);
    }

    [Test]
    public void Judge_DrawAll_ReturnsDrawAll()
    {
        var hands = new List<Hand>
        {
            new Hand(HandType.Rock, HandPosType.LeftUp),
            new Hand(HandType.Scissors, HandPosType.LeftDown),
            new Hand(HandType.Paper, HandPosType.RightUp),
        };

        var result = HandJudger.Judge(hands).ToList();

        Assert.AreEqual(3, result.Count);
        Assert.AreEqual(ResultType.DrawAll, result[0].Result);
        Assert.AreEqual(ResultType.DrawAll, result[1].Result);
        Assert.AreEqual(ResultType.DrawAll, result[2].Result);
    }

    [Test]
    public void Judge_WinLose_ReturnsCorrectResults()
    {
        var hands = new List<Hand>
        {
            new Hand(HandType.Rock, HandPosType.LeftUp),
            new Hand(HandType.Scissors, HandPosType.RightUp),
        };

        var result = HandJudger.Judge(hands).OrderBy(r => r.Hand.Pos).ToList();

        // LeftUp: Rock (Win), RightUp: Scissors (Lose)
        Assert.AreEqual(ResultType.Win, result[0].Result);
        Assert.AreEqual(ResultType.Lose, result[1].Result);
    }
}
