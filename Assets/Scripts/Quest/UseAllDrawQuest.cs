using System.Linq;

public class UseAllDrawQuest : QuestBase
{
    public UseAllDrawQuest(
        HandType targetHand,
        HandPosType targetHandPos)
    : base(targetHand, targetHandPos)
    {
    }

    public override bool InternalJudge()
    {
        return MatchTargetPosPair.Result == ResultType.DrawAll;
    }

    public override string ToString()
    {
        return $"グチョパであいこにしろ";
    }
}
