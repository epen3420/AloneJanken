using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DrawAllQuest", menuName = "Quests/DrawAll")]
public class DrawAllQuest : JankenQuestBase
{
    protected override bool InternalJudge(List<HandResultTypePair> handResultPairs)
    {
        return handResultPairs.All(pair => pair.Result == ResultType.DrawAll);
    }
}
