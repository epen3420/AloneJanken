using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LooseLeastUseOneQuest", menuName = "Quests/LooseLeastUseOne")]
public class LooseLeastUseOneQuest : JankenQuestBase
{
    [SerializeField]
    private HandType handType;


    protected override bool InternalJudge(List<HandResultTypePair> handResultPairs)
    {
        return handResultPairs.Any(hand => hand.Hand.pair.HandType == handType && hand.Result == ResultType.Lose);
    }
}
