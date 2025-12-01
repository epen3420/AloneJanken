using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DrawQuest", menuName = "Quests/Draw")]
public class DrawQuest : JankenQuestBase
{
    protected override bool InternalJudge(List<HandResultTypePair> handResultPairs)
    {
        return handResultPairs.All(pair => pair.Result == ResultType.Draw);
    }
}
