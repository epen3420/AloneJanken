using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestDB", menuName = "Quests/Database")]
public class QuestDatabase : ScriptableObject
{
    [SerializeField]
    private QuestType[] questTypes;
    [SerializeField]
    private HandPosType[] useableHandPosTypes;

    public QuestType[] QuestTypes => questTypes;
    public HandPosType[] UseableHandPotTypes => useableHandPosTypes;

    public QuestType GetQuestTypeRandomly()
    {
        int randomInt = Random.Range(0, questTypes.Length);
        return questTypes[randomInt];
    }
}
