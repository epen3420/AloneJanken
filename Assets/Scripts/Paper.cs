public class Paper : Hand
{
    public override HandType Type => HandType.Paper;
    public override HandType StrongHand => HandType.Rock;
    public override HandType WeekHand => HandType.Scissors;
}
