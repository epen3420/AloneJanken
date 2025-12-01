public enum HandPosType
{
    None = -1,
    LeftUp,
    LeftDown,
    RightUp,
    RightDown
}

public enum HandType
{
    None = -1,
    Rock,
    Scissors,
    Paper,
}

[System.Serializable]
public class Hand
{
    public readonly HandPosType OwnerPos;
    public readonly HandType HandType;

    public Hand(HandType type, HandPosType pos)
    {
        OwnerPos = pos;
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
