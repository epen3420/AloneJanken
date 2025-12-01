using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DrawOneQuest", menuName = "Quests/DrawOne")]
public class DrawOneQuest : JankenQuestBase
{
    protected override bool InternalJudge(List<HandResultTypePair> handResultPairs)
    {
        return handResultPairs.All(pair => pair.Hand.pair.HandType == TargetHand);
    }
}
