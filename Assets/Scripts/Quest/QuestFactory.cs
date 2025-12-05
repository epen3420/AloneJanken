using Debug = UnityEngine.Debug;

public enum QuestType
{
    None,
    OnlyWin,
    OnlyLose,
    LeastWin,
    LeastLose,
    UseAllDraw,
    OnlyUseDraw,
    LeastDraw,
}

public static class QuestFactory
{
    public static QuestBase GetQuestByType(
        QuestType questType,
        HandType targetHand,
        HandPosType targetHandPos)
    {
        switch (questType)
        {
            case QuestType.OnlyWin:
                return new OnlyWinQuest(targetHand, targetHandPos);
            case QuestType.OnlyLose:
                return new OnlyLoseQuest(targetHand, targetHandPos);
            case QuestType.LeastWin:
                return new LeastWinQuest(targetHand, targetHandPos);
            case QuestType.LeastLose:
                return new LeastLoseQuest(targetHand, targetHandPos);
            case QuestType.UseAllDraw:
                return new UseAllDrawQuest(targetHand, targetHandPos);
            case QuestType.OnlyUseDraw:
                return new OnlyUseDrawQuest(targetHand, targetHandPos);
            case QuestType.LeastDraw:
                return new LeastDrawQuest(targetHand, targetHandPos);

            default:
                Debug.LogWarning($"None of {questType} Quest");
                return null;
        }
    }
}
