using System.Linq;

public class OnlyUseDrawQuest : QuestBase
{
    public OnlyUseDrawQuest(
        HandType targetHand,
        HandPosType targetHandPos)
    : base(targetHand, targetHandPos)
    {
    }

    public override bool IsAllTarget => true;

    public override bool InternalJudge()
    {
        var inputPairs = UnMatchTargetPosPairs.Append(MatchTargetPosPair);

        return inputPairs.All(pair => pair.Hand.pair.HandType == TargetHand);
    }

    public override string ToString()
    {
        return $"{TargetHandName}だけであいこにしろ";
    }
}
