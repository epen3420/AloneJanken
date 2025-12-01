using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DrawLeastUseOneQuest", menuName = "Quests/DrawLeastUseOne")]
public class DrawLeastUseOneQuest : JankenQuestBase
{
    [SerializeField]
    private HandType handType;


    protected override bool InternalJudge(List<HandResultTypePair> handResultPairs)
    {
        var drawHandList = handResultPairs.Where(pair => pair.Result == ResultType.DrawOne || pair.Result == ResultType.DrawAll);
        return drawHandList.Any(hand => hand.Hand.pair.HandType == handType);
    }
}
