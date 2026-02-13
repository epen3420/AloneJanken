
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class RoundJudgeTest
{
    private class TestQuest : QuestBase
    {
        public bool JudgeResult { get; set; } = true;

        public TestQuest(HandType target, HandPosType pos) : base(target, pos) { }

        public override bool InternalJudge()
        {
            return JudgeResult;
        }

        public override string ToString() => "TestQuest";
    }

    [Test]
    public void Judge_CountMismatch_ReturnsLose_AndFillsMissingHands()
    {
        var judge = new RoundJudge();
        var quest = new TestQuest(HandType.Rock, HandPosType.LeftUp);
        var useablePos = new List<HandPosType> { HandPosType.LeftUp, HandPosType.RightUp };
        var inputHands = new List<Hand>
        {
            new Hand(HandType.Rock, HandPosType.LeftUp)
        };

        var result = judge.Judge(quest, useablePos, inputHands);

        Assert.IsFalse(result.IsWin);
        Assert.AreEqual(2, result.FinalHands.Count);

        // Assert that RightUp is now present and is Strange
        var filledHand = result.FinalHands.FirstOrDefault(h => h.Pos == HandPosType.RightUp);
        Assert.IsNotNull(filledHand);
        Assert.AreEqual(HandType.Strange, filledHand.Type);
    }

    [Test]
    public void Judge_CountMatch_AndQuestMet_ReturnsWin()
    {
        var judge = new RoundJudge();
        var quest = new TestQuest(HandType.Rock, HandPosType.LeftUp);
        quest.JudgeResult = true;

        var useablePos = new List<HandPosType> { HandPosType.LeftUp, HandPosType.RightUp };
        var inputHands = new List<Hand>
        {
            new Hand(HandType.Rock, HandPosType.LeftUp),
            new Hand(HandType.Rock, HandPosType.RightUp)
        };

        var result = judge.Judge(quest, useablePos, inputHands);

        Assert.IsTrue(result.IsWin);
        Assert.AreEqual(2, result.FinalHands.Count); // Should remain same
        Assert.AreEqual(2, result.JudgedHands.Count); // Should be judged
    }

    [Test]
    public void Judge_CountMatch_AndQuestNotMet_ReturnsLose()
    {
        var judge = new RoundJudge();
        var quest = new TestQuest(HandType.Rock, HandPosType.LeftUp);
        quest.JudgeResult = false;

        var useablePos = new List<HandPosType> { HandPosType.LeftUp };
        var inputHands = new List<Hand>
        {
             new Hand(HandType.Rock, HandPosType.LeftUp)
        };

        var result = judge.Judge(quest, useablePos, inputHands);

        Assert.IsFalse(result.IsWin);
    }
}
