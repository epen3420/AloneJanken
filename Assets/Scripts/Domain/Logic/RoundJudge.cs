
using System.Collections.Generic;
using System.Linq;

public class RoundResult
{
    public bool IsWin { get; }
    public IReadOnlyList<Hand> FinalHands { get; }
    public IReadOnlyList<HandResultTypePair> JudgedHands { get; }

    public RoundResult(bool isWin, IEnumerable<Hand> finalHands, IEnumerable<HandResultTypePair> judgedHands)
    {
        IsWin = isWin;
        FinalHands = finalHands.ToList();
        JudgedHands = judgedHands?.ToList();
    }
}

public class RoundJudge
{
    public RoundResult Judge(QuestBase quest, IEnumerable<HandPosType> useablePos, IEnumerable<Hand> inputHands)
    {
        var inputList = inputHands.ToList();
        var useableList = useablePos.ToList();

        // 1. Check if input count matches expected count
        if (inputList.Count != useableList.Count)
        {
            // Count mismatch: Fill missing with Strange and return Lose
            var filledHands = FillMissingHands(inputList, useableList);
            return new RoundResult(false, filledHands, null);
        }

        // 2. Count matches: Proceed to standard judgment
        var resultHands = HandJudger.Judge(inputList);
        bool isWin = quest.Judge(resultHands);

        return new RoundResult(isWin, inputList, resultHands);
    }

    private List<Hand> FillMissingHands(List<Hand> currentHands, List<HandPosType> useablePos)
    {
        var filledHands = new List<Hand>(currentHands);
        var currentPosList = currentHands.Select(h => h.Pos).ToHashSet();

        foreach (var pos in useablePos)
        {
            if (!currentPosList.Contains(pos))
            {
                filledHands.Add(new Hand(HandType.Strange, pos));
            }
        }
        return filledHands;
    }
}
