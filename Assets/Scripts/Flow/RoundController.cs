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

    private RoundJudge roundJudge = new RoundJudge();

    private void EndJanken()
    {
        timelineManager.EndJanken -= EndJanken;

        var result = roundJudge.Judge(currentQuest, useableHandPos, inputHands);

        // 結果を反映 (不足分の補完など)
        inputHands = result.FinalHands.ToList();

        // 入力完了イベント通知
        endInput.Raise(inputHands);

        if (result.JudgedHands != null)
        {
            foreach (var hand in result.JudgedHands)
            {
                Debug.Log($"{hand}");
            }
        }
        else
        {
            Debug.Log($"入力キーの数が手の数と異なります input: {inputHands.Count}");
        }

        Debug.Log($"Win: {result.IsWin}");
        endJanken.Raise(result.IsWin);
        inputHands.Clear();
    }
}
