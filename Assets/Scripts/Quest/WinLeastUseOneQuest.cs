using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WinLeastUseOneQuest", menuName = "Quests/WinLeastUseOne")]
public class WinLeastUseOneQuest : JankenQuestBase
{
    protected override bool InternalJudge(List<HandResultTypePair> handResultPairs)
    {
        for (int i = 0; i < handResultPairs.Count; i++)
        {
            if (handResultPairs[i].Hand.pair.HandType == TargetHand &&
                handResultPairs[i].Result == ResultType.Win)
            {
                handResultPairs.RemoveAt(i);
                break;
            }
        }

        return handResultPairs.All(pair => pair.Result == ResultType.Lose);
    }
}
