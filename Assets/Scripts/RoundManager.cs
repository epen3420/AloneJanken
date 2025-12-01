using System.Threading;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    private int jankenBpm = 60;
    [SerializeField]
    private int beatsNum = 7;
    [SerializeField]
    private JankenInputManager inputs;

    [Header("Events")]
    [SerializeField]
    private QuestEventChannelSO startRound;
    [SerializeField]
    private BoolEventChannelSO endRound;
    [SerializeField]
    private FloatEventChannelSO changeTimeEvent;

    [SerializeField]
    private QuestDatabase questDb;

    private CountDownTimer timer;
    private bool isPlaying = false;

    private void Awake()
    {
        timer = new CountDownTimer(changeTimeEvent, null);
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
            quest.LotteryTargetHandPos();

        // キャンセルされているかチェック
        ctn.ThrowIfCancellationRequested();

        startRound.Raise(quest);

        inputs.Enable();
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

        var inputResult = inputs.GetCurrentInputHand();

        bool isWin = false;
        if (inputResult.Count() == HandTypeUtil.HandPosCount)
        {
            var resultHands = HandJudger.Judge(inputResult);
            isWin = quest.Judge(resultHands);

            foreach (var hand in resultHands)
            {
                Debug.Log($"Hand: {hand}");
            }
        }

        Debug.Log(isWin);
        endRound.Raise(isWin);
    }
}
