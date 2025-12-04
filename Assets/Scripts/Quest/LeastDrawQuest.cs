using System.Linq;

public class LeastDrawQuest : QuestBase
{
    public LeastDrawQuest(
        HandType targetHand,
        HandPosType targetHandPos)
    : base(targetHand, targetHandPos)
    {
    }

    public override bool InternalJudge()
    {
        return MatchTargetPosPair.Result == ResultType.DrawOne ||
               MatchTargetPosPair.Result == ResultType.DrawAll;
    }

    public override string ToString()
    {
        return $"{TargetHandName}であいこにしろ";
    }
}
