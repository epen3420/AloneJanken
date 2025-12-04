using System.Collections.Generic;
using System.Linq;

public abstract class QuestBase
{
    private readonly HandType targetHand;
    private readonly HandPosType targetHandPos;
    private HandResultTypePair matchTargetPosPair;
    private IEnumerable<HandResultTypePair> unMatchTargetPosPairs;

    protected string TargetHandName => HandTypeUtil.GetHandName(targetHand);
    protected string TargetHandPosName => HandTypeUtil.GetHandPosName(targetHandPos);
    protected HandResultTypePair MatchTargetPosPair => matchTargetPosPair;
    protected IEnumerable<HandResultTypePair> UnMatchTargetPosPairs => unMatchTargetPosPairs;

    public HandType TargetHand => targetHand;
    public HandPosType TargetHandPos => targetHandPos;

    public QuestBase(
        HandType targetHand,
        HandPosType targetHandPos)
    {
        this.targetHand = targetHand;
        this.targetHandPos = targetHandPos;
    }

    protected virtual bool IsAllTarget { get; } = false;

    public abstract bool InternalJudge();
    public abstract override string ToString();

    public bool Judge(IEnumerable<HandResultTypePair> handResultPairs)
    {
        matchTargetPosPair = handResultPairs.First(pairs => pairs.Hand.pair.OwnerPos == targetHandPos);

        unMatchTargetPosPairs = handResultPairs.Where(pairs => pairs.Hand.pair.OwnerPos != targetHandPos);

        return InternalJudge();
    }
}
