using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text questText;
    [SerializeField]
    private TMP_Text targetHandText; // 仮置きいずれアウトラインみたいな実装になる
    [Header("Events")]
    [SerializeField]
    private QuestEventChannelSO startRound;
    [SerializeField]
    private BoolEventChannelSO endRound;
    [SerializeField]
    private FloatEventChannelSO changeTimeEvent;


    private void OnEnable()
    {
        startRound.OnRaised += ShowQuest;
        startRound.OnRaised += ShowTargetHand;
    }

    private void OnDisable()
    {
        startRound.OnRaised -= ShowQuest;
        startRound.OnRaised -= ShowTargetHand;
    }

    private void ShowQuest(QuestBase quest)
    {
        questText.SetText(quest.ToString());
    }

    private void ShowTargetHand(QuestBase quest)
    {
        targetHandText.SetText($"{HandTypeUtil.GetHandPosName(quest.TargetHandPos)}");
    }
}
