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
    private TMP_Dropdown targetPosDropdown;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private GameCycleManager cycleManager;
    [SerializeField]
    private QuestDatabase questDb;


    private void Start()
    {
        var questOptions = new List<TMP_Dropdown.OptionData>(questDb.Quests.Select(h => new TMP_Dropdown.OptionData(h.QuestName)));
        questDropdown.options = questOptions;

        var targetPosOptions = new List<TMP_Dropdown.OptionData>(HandTypeUtil.HandPosTypes.Select(h => new TMP_Dropdown.OptionData(HandTypeUtil.GetHandPosName(h))));
        targetPosDropdown.options = targetPosOptions;

        startButton.onClick.AddListener(async () => await cycleManager.StartRound(questDb.Quests[questDropdown.value], destroyCancellationToken, HandTypeUtil.HandPosTypes[targetPosDropdown.value]));
    }
}
