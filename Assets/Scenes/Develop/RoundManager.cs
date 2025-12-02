/* using System.Threading;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    private int jankenBpm = 60;
    [SerializeField]
    private int beatsNum = 7;
    [SerializeField]
    private JankenInputManager inputs;
    // Test
    [SerializeField]
    private TMP_Text logText;
    [SerializeField]
    private TMP_Dropdown targetHandDropdown;
    [SerializeField]
    private TMP_Dropdown questDropdown;
    [SerializeField]
    private Button startButton;
    //

    [Header("Events")]
    [SerializeField]
    private QuestEventChannelSO startRound;
    [SerializeField]
    private BoolEventChannelSO endRound;
    [SerializeField]
    private FloatEventChannelSO changeTimeEvent;
    [SerializeField]
    private HandsEventChannelSO inputHandsEvent;

    [SerializeField]
    private QuestDatabase questDb;

    private CountDownTimer timer;
    private bool isPlaying = false;

    private void Awake()
    {
        timer = new CountDownTimer(changeTimeEvent, null);

        // Test
        var options = questDb.Quests.Select(quest => new TMP_Dropdown.OptionData(quest.QuestName));
        questDropdown.AddOptions(options.ToList());

        var targetHandOptions = HandTypeUtil.HandPosTypes.Select(hand => new TMP_Dropdown.OptionData(HandTypeUtil.GetHandPosName(hand)));
        targetHandDropdown.AddOptions(targetHandOptions.ToList());

        startButton.onClick.AddListener(async () => await StartRound(questDb.Quests[questDropdown.value], destroyCancellationToken));
    }

    public async UniTaskVoid GameCycle(CancellationToken ctn)
    {
        if (isPlaying) return;
        isPlaying = true;

        int totalRounds = questDb.Quests.Length;
        try
        {
            for (int roundCount = 0; roundCount < totalRounds; roundCount++)
            {
                await StartRound(questDb.Quests[roundCount], ctn);
            }
        }
        finally
        {
            isPlaying = false;
        }
    }

    private async UniTask StartRound(JankenQuestBase quest, CancellationToken ctn)
    {
        if (!quest.IsAllTarget)
        {
            var randomTargetHandPos = HandTypeUtil.HandPosTypes[Random.Range(0, HandTypeUtil.HandPosCount)];

            // quest.SetTargetHandPos(randomTargetHandPos);
            quest.SetTargetHandPos((HandPosType)targetHandDropdown.value);
        }
        // キャンセルされているかチェック
        ctn.ThrowIfCancellationRequested();

        logText.SetText("");
        startRound.Raise(quest);

        inputs.Enable();

        Hand[] inputResult;
        try
        {
            float duration = 60f / jankenBpm * beatsNum;
            timer.Init(duration);

            await timer.Resume(ctn);
        }
        finally
        {
            inputs.Disable();
        }


        bool isWin = false;
        if (inputResult.Length == HandTypeUtil.HandPosCount)
        {
            var resultHands = HandJudger.Judge(inputResult);
            isWin = quest.Judge(resultHands);

            foreach (var hand in resultHands)
            {
                logText.text += $"{hand}\n";
            }
        }
        else
        {
            logText.text += $"入力キーの数が手の数と異なります\n\tinput: {inputResult.Length}\n\n";
        }

        logText.text += $"Win: {isWin}\n";
        endRound.Raise(isWin);
    }
}
 */
