using System.Collections.Generic;
using System.Linq;

public enum ResultType
{
    None = -1,
    Draw,
    Win,
    Lose,
}

public static class HandJudger
{
    public static IEnumerable<HandResultTypePair> Judge(IEnumerable<Hand> hands)
    {
        var handList = hands.ToList();

        // 重複を削除
        var uniqueHandTypes = handList.Select(h => h.pair.HandType)
                                      .Distinct()
                                      .ToList();

        int uniqueHandTypeCount = uniqueHandTypes.Count();

        var results = new List<HandResultTypePair>();
        // 一種類か三種類全部か
        if (uniqueHandTypeCount == 1 ||
           uniqueHandTypeCount == 3)
        {
            return handList.Select(hand => new HandResultTypePair(hand, ResultType.Draw));
        }

        // この先実質二人でのじゃんけん
        // + あいこは存在しない
        var typeA = uniqueHandTypes[0];
        var typeB = uniqueHandTypes[1];

        var handA = handList.FirstOrDefault(h => h.pair.HandType == typeA);
        var isWinHandA = handA.IsWin(typeB);
        var winnerHandType = isWinHandA ? typeA : typeB;

        return handList.Select(h =>
            new HandResultTypePair
            (
                h,
                (h.pair.HandType == winnerHandType) ? ResultType.Win : ResultType.Lose
            )
        );
    }
}
