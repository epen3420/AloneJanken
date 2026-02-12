

[System.Serializable]
public class Hand : System.IEquatable<Hand>
{
    [UnityEngine.SerializeField] private HandType type;
    [UnityEngine.SerializeField] private HandPosType pos;

    public HandType Type => type;
    public HandPosType Pos => pos;

    public Hand(HandType type, HandPosType pos)
    {
        this.type = type;
        this.pos = pos;
    }

    /// <summary>
    /// この手が enemyHand に対して勝っているかどうかを判定する
    /// </summary>
    public bool IsWin(HandType enemyHand)
    {
        if (type == HandType.Rock && enemyHand == HandType.Scissors) return true;
        if (type == HandType.Scissors && enemyHand == HandType.Paper) return true;
        if (type == HandType.Paper && enemyHand == HandType.Rock) return true;
        return false;
    }

    public bool Equals(Hand other)
    {
        if (other is null) return false;
        return this.type == other.type && this.pos == other.pos;
    }

    public override bool Equals(object obj) => Equals(obj as Hand);
    public override int GetHashCode() => (type, pos).GetHashCode();
}
