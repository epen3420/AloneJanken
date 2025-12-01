using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class JankenQuestBase : ScriptableObject
{
    [SerializeField]
    private string questName;
    [SerializeField]
    private string description;
    [SerializeField]
    private bool isAllTarget;
    [SerializeField]
    private HandType targetHand;


    protected abstract bool InternalJudge(List<HandResultTypePair> handResultPairs);

    private HandPosType currentTargetHandPos;

    public HandPosType CurrentTargetHandPos => currentTargetHandPos;
    public HandType TargetHand => targetHand;
    public bool IsAllTarget => isAllTarget;

    public void LotteryTargetHandPos()
    {
        currentTargetHandPos = HandTypeUtil.GetHandPosTypes()[Random.Range(0, HandTypeUtil.HandPosCount)];
    }

    public bool Judge(IEnumerable<HandResultTypePair> handResultPairs)
    {
        if (!isAllTarget &&
            !handResultPairs.Any(hand => hand.Hand.pair.OwnerPos == currentTargetHandPos && hand.Hand.pair.HandType == targetHand))
        {
            return false;
        }

        var pairs = handResultPairs.ToList();
        var uniqueHandPosCount = pairs.Select(pair => pair.Hand.pair.OwnerPos)
                                      .Distinct()
                                      .Count();

        // 同じ手を使っていたらミス
        if (pairs.Count > uniqueHandPosCount)
        {
            return false;
        }

        return InternalJudge(pairs);
    }
}
