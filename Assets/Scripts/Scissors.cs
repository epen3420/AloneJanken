public class Scissors : Hand
{
    public override HandType HandType => HandType.Scissors;
    public override HandType StrongHand => HandType.Paper;
    public override HandType WeekHand => HandType.Rock;
}
