using System.Threading;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class RoundController : MonoBehaviour
{
    [SerializeField]
    private int bpm = 60;
    [SerializeField]
    private int beatsNum = 8;
    [Header("Events")]
    [SerializeField]
    private QuestEventChannelSO startRound;
    [SerializeField]
    private BoolEventChannelSO endJanken;
    [SerializeField]
    private IntEventChannelSO changeBeats;
    [SerializeField]
    private VoidEventChannelSO endBeats;
    [SerializeField]
    private HandsEventChannelSO inputEvent;
    [SerializeField]
    private HandsEventChannelSO endInput;
    [SerializeField]
    private TimelineManager timelineManager;

    private QuestBase currentQuest;
    private List<HandPosType> useableHandPos;
    private List<Hand> inputHands = new List<Hand>();


    private void OnEnable()
    {
        // changeBeats.OnRaised += EndJanken;
        inputEvent.OnRaised += SetInputHands;
    }

    private void OnDisable()
    {
        // changeBeats.OnRaised -= EndJanken;
        inputEvent.OnRaised -= SetInputHands;
    }

    private void SetInputHands(IEnumerable<Hand> inputHands)
    {
        this.inputHands = inputHands.ToList();
    }

    public async UniTask StartRound(
        QuestBase quest,
        IEnumerable<HandPosType> useableHandPos,
        CancellationToken ctn)
    {
        // キャンセルされているかチェック
        ctn.ThrowIfCancellationRequested();

        currentQuest = quest;
        this.useableHandPos = useableHandPos.ToList();

        Debug.Log($"{currentQuest.ToString()}");

        startRound.Raise(currentQuest);

        await timelineManager.Execute(ctn);
    }

    public void EndJanken()
    {
        bool isWin = CheckWin();

        Debug.Log($"Win: {isWin}");
        endJanken.Raise(isWin);
        inputHands.Clear();
    }

    private bool CheckWin()
    {
        bool isWin = false;
        if (inputHands.Count == useableHandPos.Count)
        {
            endInput.Raise(inputHands);
            var resultHands = HandJudger.Judge(inputHands);
            isWin = currentQuest.Judge(resultHands);

            foreach (var hand in resultHands)
            {
                Debug.Log($"{hand}");
            }
        }
        else
        {
            Debug.Log($"入力キーの数が手の数と異なります input: {inputHands.Count}");
            var inputHandPosList = inputHands.Select(hand => hand.pair.OwnerPos).ToList();
            foreach (var handPos in useableHandPos)
            {
                if (!inputHandPosList.Contains(handPos))
                {
                    inputHands.Add(new Hand(HandType.Strange, handPos));
                }
            }
            endInput.Raise(inputHands);
        }

        return isWin;
    }
}
