using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WinLeastUseOneQuest", menuName = "Quests/WinLeastUseOne")]
public class WinLeastUseOneQuest : JankenQuestBase
{
    protected override bool InternalJudge(List<HandResultTypePair> handResultPairs)
    {
        return handResultPairs.Any(pair => pair.Hand.pair.HandType == TargetHand && pair.Result == ResultType.Win);
    }
}
