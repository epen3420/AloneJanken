using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LooseLeastUseOneQuest", menuName = "Quests/LooseLeastUseOne")]
public class LooseLeastUseOneQuest : JankenQuestBase
{
    protected override bool InternalJudge(List<HandResultTypePair> handResultPairs)
    {
        return handResultPairs.Any(hand => hand.Hand.pair.HandType == TargetHand && hand.Result == ResultType.Lose);
    }
}
