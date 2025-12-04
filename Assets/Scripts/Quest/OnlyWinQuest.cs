using System.Linq;

public class OnlyWinQuest : QuestBase
{
    public OnlyWinQuest(
        HandType targetHand,
        HandPosType targetHandPos)
    : base(targetHand, targetHandPos)
    {
    }

    public override bool InternalJudge()
    {
        return MatchTargetPosPair.Result == ResultType.Win &&
               UnMatchTargetPosPairs.All(pairs => pairs.Result == ResultType.Lose);
    }

    public override string ToString()
    {
        return $"{TargetHandName}でひとり勝ちしろ";
    }
}
