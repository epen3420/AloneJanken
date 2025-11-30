using System.Collections.Generic;
using System.Linq;

public enum ResultType
{
    None = -1,
    Draw,
    Win,
    Lose,
}

public struct HandResultTypePair
{
    public ResultType result;
    public HandType winHandType;
}

public class HandJudger
{
    public HandResultTypePair Judge(IEnumerable<Hand> hands)
    {
        // 重複を削除
        var uniqueHands = hands.Distinct();
        int uniqueHandCount = uniqueHands.Count();

        // 一種類か三種類全部か
        if (uniqueHandCount == 1 ||
           uniqueHandCount == 3)
        {
            var result = new HandResultTypePair
            {
                result = ResultType.Draw,
                winHandType = HandType.None,
            };
            return ResultType.Draw;
        }

        var me = uniqueHands.ElementAt(0);
        var enemy = uniqueHands.ElementAt(1);

        return Judge(me, enemy);
    }

    public ResultType Judge(Hand me, Hand enemy)
    {
        if (me.HandType == enemy.HandType)
        {
            return ResultType.Draw;
        }
        else if (me.HandType == enemy.WeekHand)
        {
            return ResultType.Win;
        }
        else if (me.HandType == enemy.StrongHand)
        {
            return ResultType.Lose;
        }
        else
        {
            return ResultType.None;
        }
    }
}
