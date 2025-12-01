[System.Serializable]
public class HandResultTypePair
{
    public HandResultTypePair(Hand hand, ResultType result)
    {
        Hand = hand;
        Result = result;
    }

    public readonly Hand Hand;
    public readonly ResultType Result;
}
