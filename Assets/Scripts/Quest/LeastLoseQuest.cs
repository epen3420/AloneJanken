using System.Linq;

public class LeastLoseQuest : QuestBase
{
    public LeastLoseQuest(
        HandType targetHand,
        HandPosType targetHandPos)
    : base(targetHand, targetHandPos)
    {
    }

    public override bool InternalJudge()
    {
        return MatchTargetPosPair.Result == ResultType.Lose;
    }

    public override string ToString()
    {
        return $"{TargetHandName}で負けろ";
    }
}
