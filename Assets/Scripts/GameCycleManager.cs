using System.Threading;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class GameCycleManager : MonoBehaviour
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
    private HandsEventChannelSO inputEvent;
    [SerializeField]
    private HandsEventChannelSO endInput;
    [SerializeField]
    private JankenResultPairEventChannelSO jankenResultPairEvent;

    [SerializeField]
    private QuestDatabase questDb;

    private CountDownTimer timer;
    private bool isPlaying = false;
    private List<Hand> inputHands = new List<Hand>();

    private void Awake()
    {
        timer = new CountDownTimer(changeTimeEvent, null);
    }

    private void OnEnable()
    {
        inputEvent.OnRaised += SetInputHands;
    }

    private void OnDisable()
    {
        inputEvent.OnRaised -= SetInputHands;
    }

    private void SetInputHands(IEnumerable<Hand> inputHands)
    {
        this.inputHands = inputHands.ToList();
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
                int randomInt = Random.Range(0, totalRounds);
                await StartRound(questDb.Quests[randomInt], ctn);
            }
        }
        finally
        {
            isPlaying = false;
        }
    }

    public async UniTask StartRound(JankenQuestBase quest, CancellationToken ctn, HandPosType targetHandPost = HandPosType.None)
    {
        if (!quest.IsAllTarget)
        {
            var randomTargetHandPos = HandTypeUtil.HandPosTypes[Random.Range(0, HandTypeUtil.HandPosCount)];

            quest.SetTargetHandPos(targetHandPost == HandPosType.None ? randomTargetHandPos : targetHandPost);
        }
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


        bool isWin = false;
        if (inputHands.Count == HandTypeUtil.HandPosCount)
        {
            endInput.Raise(inputHands);
            var resultHands = HandJudger.Judge(inputHands);
            jankenResultPairEvent.Raise(resultHands);
            isWin = quest.Judge(resultHands);

            foreach (var hand in resultHands)
            {
                Debug.Log($"{hand}");
            }
        }
        else
        {
            var inputHandPosList = inputHands.Select(hand => hand.pair.OwnerPos).ToList();
            foreach (var handPos in HandTypeUtil.HandPosTypes)
            {
                if (!inputHandPosList.Contains(handPos))
                {
                    inputHands.Add(new Hand(HandType.Strange, handPos));
                }
            }
            endInput.Raise(inputHands);
            Debug.Log($"入力キーの数が手の数と異なります input: {inputHands.Count}");
        }

        Debug.Log($"Win: {isWin}");
        endRound.Raise(isWin);
        inputHands.Clear();
    }
}
