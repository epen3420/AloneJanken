using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestDB", menuName = "Quests/Database")]
public class QuestDatabase : ScriptableObject
{
    [SerializeField]
    private JankenQuestBase[] quests;

    public JankenQuestBase[] Quests => quests;
}
