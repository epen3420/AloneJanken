using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestDB", menuName = "Quests/Database")]
public class QuestDatabase : ScriptableObject
{
    [SerializeField]
    private QuestType[] questTypes;

    public QuestType[] QuestTypes => questTypes;

    public QuestType GetQuestTypeRandomly()
    {
        int randomInt = Random.Range(0, questTypes.Length);
        return questTypes[randomInt];
    }
}
