using System.Collections.Generic;
using System.Linq;



public static class HandJudger
{
    public static IEnumerable<HandResultTypePair> Judge(IEnumerable<Hand> hands)
    {
        var handList = hands.ToList();

        // 重複を削除
        var uniqueHandTypes = handList.Select(h => h.Type)
                                      .Distinct()
                                      .ToList();

        int uniqueHandTypeCount = uniqueHandTypes.Count();

        // 一種類か三種類全部か
        if (uniqueHandTypeCount == 1)
        {
            return handList.Select(hand => new HandResultTypePair(hand, ResultType.DrawOne));
        }

        if (uniqueHandTypeCount == 3)
        {
            return handList.Select(hand => new HandResultTypePair(hand, ResultType.DrawAll));
        }

        // この先実質二人でのじゃんけん
        // + あいこは存在しない
        var typeA = uniqueHandTypes[0];
        var typeB = uniqueHandTypes[1];

        // typeA が typeB に勝つかどうか
        // HandインスタンスがなくてもHandTypeだけで勝敗判定できるロジックがあれば便利だが
        // ここでは仮の手を作って判定するか、ロジックを分離するか。
        // Hand.IsWinはインスタンスメソッドだが、staticな比較メソッドも欲しいところ。
        // しかし現状はインスタンスから判定する。
        // typeAを持つ最初の手を探す
        var handA = handList.First(h => h.Type == typeA);
        var isWinHandA = handA.IsWin(typeB);
        var winnerHandType = isWinHandA ? typeA : typeB;

        return handList.Select(h =>
            new HandResultTypePair
            (
                h,
                (h.Type == winnerHandType) ? ResultType.Win : ResultType.Lose
            )
        );
    }
}
