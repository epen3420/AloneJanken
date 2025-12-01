public enum HandType
{
    None = -1,
    Rock,
    Scissors,
    Paper,
}

public class Hand
{
    public readonly string OwnerName;
    public readonly HandType HandType;

    public Hand(HandType type, string name = "Player")
    {
        OwnerName = name;
        HandType = type;
    }

    /// <summary>
    /// thisインスタンスから見てenemyHandに対して勝っているかどうかを判定する
    /// あいこの場合はfalseが返る
    /// </summary>
    /// <param name="enemyHand"></param>
    /// <returns></returns>
    public bool IsWin(HandType enemyHand)
    {
        var meHand = this.HandType;

        if (meHand == HandType.Rock && enemyHand == HandType.Scissors) return true;
        if (meHand == HandType.Scissors && enemyHand == HandType.Paper) return true;
        if (meHand == HandType.Paper && enemyHand == HandType.Rock) return true;

        return false;
    }
}
