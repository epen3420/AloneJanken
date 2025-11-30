public enum ResultType
{
    None = -1,
    Draw,
    Win,
    Lose,
}
public class HandJudger
{
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
