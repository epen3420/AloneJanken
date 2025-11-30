public enum HandType
{
    None = -1,
    Rock,
    Scissors,
    Paper,
}

public abstract class Hand
{
    public abstract HandType HandType { get; }
    public abstract HandType StrongHand { get; }
    public abstract HandType WeekHand { get; }
}
