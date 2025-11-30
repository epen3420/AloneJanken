public class Paper : Hand
{
    public override HandType HandType => HandType.Paper;
    public override HandType StrongHand => HandType.Rock;
    public override HandType WeekHand => HandType.Scissors;
}
