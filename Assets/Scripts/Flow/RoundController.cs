using System.Threading;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class RoundController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField]
    private QuestEventChannelSO startRound;
    [SerializeField]
    private BoolEventChannelSO endJanken;
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

        timelineManager.EndJanken += EndJanken;
        await timelineManager.Execute(ctn);
    }

    private void EndJanken()
    {
        timelineManager.EndJanken -= EndJanken;

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
            var inputHandPosList = inputHands.Select(hand => hand.Pos).ToList();
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
