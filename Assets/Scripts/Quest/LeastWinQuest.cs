using System.Linq;

public class LeastWinQuest : QuestBase
{
    public LeastWinQuest(
        HandType targetHand,
        HandPosType targetHandPos)
    : base(targetHand, targetHandPos)
    {
    }

    public override bool InternalJudge()
    {
        return MatchTargetPosPair.Result == ResultType.Win;
    }

    public override string ToString()
    {
        return $"{TargetHandName}で勝て";
    }
}
