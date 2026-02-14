
using System.Collections.Generic;
using NUnit.Framework;

public class QuestTest
{
    [Test]
    public void LeastDrawQuest_Judge_ReturnsTrue_WhenConditionMet()
    {
        // Target: Rock, LeftUp. Condition: DrawOne or DrawAll
        var quest = new LeastDrawQuest(HandType.Rock, HandPosType.LeftUp);

        // Case 1: DrawOne (Rock vs Rock)
        var hands = new List<Hand>
        {
            new Hand(HandType.Rock, HandPosType.LeftUp),
            new Hand(HandType.Rock, HandPosType.RightUp), // Draw
        };
        var results = HandJudger.Judge(hands);
        Assert.IsTrue(quest.Judge(results));
    }

    [Test]
    public void OnlyWinQuest_Judge_ReturnsTrue_WhenConditionMet()
    {
        // Target: Rock, LeftUp. Condition: Win AND others Lose
        var quest = new OnlyWinQuest(HandType.Rock, HandPosType.LeftUp);

        var hands = new List<Hand>
        {
            new Hand(HandType.Rock, HandPosType.LeftUp),     // Win
            new Hand(HandType.Scissors, HandPosType.RightUp), // Lose
        };
        var results = HandJudger.Judge(hands);
        Assert.IsTrue(quest.Judge(results));
    }

    [Test]
    public void OnlyLoseQuest_Judge_ReturnsTrue_WhenConditionMet()
    {
        // Target: Scissors, LeftUp. Condition: Lose AND others Win
        var quest = new OnlyLoseQuest(HandType.Scissors, HandPosType.LeftUp);

        var hands = new List<Hand>
        {
            new Hand(HandType.Scissors, HandPosType.LeftUp), // Lose
            new Hand(HandType.Rock, HandPosType.RightUp),    // Win
        };
        var results = HandJudger.Judge(hands);
        Assert.IsTrue(quest.Judge(results));
    }

    [Test]
    public void OnlyUseDrawQuest_Judge_ReturnsTrue_WhenAllHandsAreTargetType()
    {
        // Target: Rock. Condition: All hands are Rock (DrawAll/One)
        var quest = new OnlyUseDrawQuest(HandType.Rock, HandPosType.LeftUp);

        var hands = new List<Hand>
        {
            new Hand(HandType.Rock, HandPosType.LeftUp),
            new Hand(HandType.Rock, HandPosType.RightUp),
            new Hand(HandType.Rock, HandPosType.LeftDown),
        };
        var results = HandJudger.Judge(hands);
        Assert.IsTrue(quest.Judge(results));
    }

    [Test]
    public void OnlyUseDrawQuest_Judge_ReturnsFalse_WhenOtherHandIsSameTypeButDifferentTargetType()
    {
       // If logic was "All are Draw", this would be true.
       // But OnlyUseDrawQuest says: "inputPairs.All(pair => pair.Hand.Type == TargetHand)"
       // So if one hand is NOT TargetHand (e.g. Paper), even if it's a Draw (Paper vs Paper), it should fail?
       // Let's check logic: matchTargetPosPair must be checked too?
       // Code: var inputPairs = UnMatchTargetPosPairs.Append(MatchTargetPosPair);
       // return inputPairs.All(pair => pair.Hand.Type == TargetHand);
       // So ALL hands must be TargetHand.

       var quest = new OnlyUseDrawQuest(HandType.Rock, HandPosType.LeftUp);
       var hands = new List<Hand>
       {
           new Hand(HandType.Paper, HandPosType.LeftUp), // Not Rock
           new Hand(HandType.Paper, HandPosType.RightUp),
       };
       var results = HandJudger.Judge(hands);
       Assert.IsFalse(quest.Judge(results));
    }
}
