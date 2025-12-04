using System.Linq;

public class OnlyLoseQuest : QuestBase
{
    public OnlyLoseQuest(
        HandType targetHand,
        HandPosType targetHandPos)
    : base(targetHand, targetHandPos)
    {
    }

    public override bool InternalJudge()
    {
        return MatchTargetPosPair.Result == ResultType.Lose &&
               UnMatchTargetPosPairs.All(pairs => pairs.Result == ResultType.Win);
    }

    public override string ToString()
    {
        return $"{TargetHandName}でひとり負けしろ";
    }
}
