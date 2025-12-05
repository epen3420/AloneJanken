public enum HandPosType
{
    None,
    LeftUp,
    LeftDown,
    RightUp,
    RightDown
}

public enum HandType
{
    None,
    Rock,
    Scissors,
    Paper,
    Strange,
}

[System.Serializable]
public class Hand
{
    [System.Serializable]
    public struct Pair
    {
        public HandPosType OwnerPos;
        public HandType HandType;
    }

    public readonly Pair pair;

    public Hand(HandType type, HandPosType pos)
    {
        pair = new Pair
        {
            OwnerPos = pos,
            HandType = type,
        };
    }

    /// <summary>
    /// thisインスタンスから見てenemyHandに対して勝っているかどうかを判定する
    /// あいこの場合はfalseが返る
    /// </summary>
    /// <param name="enemyHand"></param>
    /// <returns></returns>
    public bool IsWin(HandType enemyHand)
    {
        var meHand = this.pair.HandType;

        if (meHand == HandType.Rock && enemyHand == HandType.Scissors) return true;
        if (meHand == HandType.Scissors && enemyHand == HandType.Paper) return true;
        if (meHand == HandType.Paper && enemyHand == HandType.Rock) return true;

        return false;
    }
}
