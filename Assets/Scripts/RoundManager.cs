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

    public async UniTask Execute(CancellationToken ctn)
    {
        if (isPlaying) return;
        isPlaying = true;

        int totalRounds = questDb.Quests.Length;
        try
        {
            for (int roundCount = 0; roundCount < totalRounds; roundCount++)
            {
                var currentQuest = questDb.Quests[roundCount];

                // キャンセルされているかチェック
                ctn.ThrowIfCancellationRequested();

                startRound.Raise(currentQuest);

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
                if (inputResult.Any())
                {
                    var resultHands = HandJudger.Judge(inputResult);
                    isWin = currentQuest.Judge(resultHands);

                    foreach (var hand in resultHands)
                    {
                        Debug.Log($"Hand: {hand}");
                    }
                }

                endRound.Raise(isWin);
            }
        }
        finally
        {
            isPlaying = false;
        }
    }
}
