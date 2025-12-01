using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class JankenQuestBase : ScriptableObject
{
    [SerializeField]
    private string questName;
    [SerializeField]
    private string description;

    protected abstract bool InternalJudge(List<HandResultTypePair> handResultPairs);

    public bool Judge(IEnumerable<HandResultTypePair> handResultPairs)
    {
        var pairs = handResultPairs.ToList();
        if (pairs.Count < 2)
        {
            return false;
        }

        var uniqueHandPosCount = pairs.Select(pair => pair.Hand.OwnerPos)
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
