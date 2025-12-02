using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LooseLeastUseOneQuest", menuName = "Quests/LooseLeastUseOne")]
public class LooseLeastUseOneQuest : JankenQuestBase
{
    protected override bool InternalJudge(List<HandResultTypePair> handResultPairs)
    {
        for (int i = 0; i < handResultPairs.Count; i++)
        {
            if (handResultPairs[i].Hand.pair.HandType == TargetHand &&
                handResultPairs[i].Result == ResultType.Lose)
            {
                handResultPairs.RemoveAt(i);
                break;
            }
        }

        return handResultPairs.All(pair => pair.Result == ResultType.Win);
    }
}
