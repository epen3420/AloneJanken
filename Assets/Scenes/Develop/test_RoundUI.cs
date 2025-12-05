using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class test_RoundUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown questDropdown;
    [SerializeField]
    private TMP_Dropdown targetHandDropdown;
    [SerializeField]
    private TMP_Dropdown targetPosDropdown;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private TMP_Text logText;
    [SerializeField]
    private VoidEventChannelSO startRound;
    [SerializeField]
    private RoundController cycleManager;
    [SerializeField]
    private QuestDatabase questDb;


    private void Start()
    {
        var questOptions = new List<TMP_Dropdown.OptionData>(questDb.QuestTypes.Select(h => new TMP_Dropdown.OptionData(h.ToString())));
        questDropdown.options = questOptions;

        var targetHandOptions = new List<TMP_Dropdown.OptionData>(HandTypeUtil.HandTypes.Select(h => new TMP_Dropdown.OptionData(HandTypeUtil.GetHandName(h))));
        targetHandDropdown.options = targetHandOptions;

        var targetPosOptions = new List<TMP_Dropdown.OptionData>(HandTypeUtil.HandPosTypes.Select(h => new TMP_Dropdown.OptionData(HandTypeUtil.GetHandPosName(h))));
        targetPosDropdown.options = targetPosOptions;

        Application.logMessageReceived += SetLogText;
        startRound.OnVoidRaised += ResetLogText;

        startButton.onClick.AddListener(async () =>
        {
            try
            {
                var questType = questDb.QuestTypes[questDropdown.value];
                var targetHand = HandTypeUtil.HandTypes[targetHandDropdown.value];
                var targetHandPos = HandTypeUtil.HandPosTypes[targetPosDropdown.value];
                Debug.Log($"{questType} : {targetHand} : {targetHandPos}");
                var quest = QuestFactory.GetQuestByType(questType, targetHand, targetHandPos);

                await cycleManager.StartRound(quest, destroyCancellationToken);
            }
            catch (System.OperationCanceledException)
            {
            }
        });
    }

    private void SetLogText(string logStr, string _, LogType logType)
    {
        if (logType == LogType.Log)
            logText.text += "- " + logStr + "\n";
    }

    private void ResetLogText()
    {
        logText.SetText("");
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= SetLogText;
    }
}
