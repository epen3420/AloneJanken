public class Rock : Hand
{
    public override HandType HandType => HandType.Rock;
    public override HandType StrongHand => HandType.Scissors;
    public override HandType WeekHand => HandType.Paper;
}
