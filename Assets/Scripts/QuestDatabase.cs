using UnityEngine;

public class QuestDatabase : ScriptableObject
{
    private JankenQuestBase[] quests;

    public JankenQuestBase[] Quests => quests;
}
