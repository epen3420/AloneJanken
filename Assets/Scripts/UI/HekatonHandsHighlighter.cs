using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class HekatonHandsHighlighter : MonoBehaviour
{
    [SerializeField]
    private QuestEventChannelSO startRound;
    [SerializeField]
    private VoidEventChannelSO endTimer;
    [SerializeField]
    private Color highlightColor;
    [SerializeField]
    private float highlightDuration = 0.5f;
    [SerializeField]
    private HandPosImageMaps[] handPosImageMaps;

    [System.Serializable]
    private struct HandPosImageMaps
    {
        public HandPosType posType;
        public Image image;
    }

    private Dictionary<HandPosType, Image> handPosImageDict = new Dictionary<HandPosType, Image>();
    private CountDownTimer timer;
    private Dictionary<HandPosType, CancellationTokenSource> stopHighlightCtsDict;


    private void Awake()
    {
        timer = new CountDownTimer(null, null);

        foreach (var map in handPosImageMaps)
        {
            handPosImageDict.Add(map.posType, map.image);
        }
    }

    private void OnEnable()
    {
        stopHighlightCtsDict = new Dictionary<HandPosType, CancellationTokenSource>();
        startRound.OnRaised += HighlightHands;
        startRound.OnVoidRaised += StartTimer;

        endTimer.OnVoidRaised += EndHighlight;
    }

    private void OnDisable()
    {
        startRound.OnRaised -= HighlightHands;
        startRound.OnVoidRaised -= StartTimer;

        endTimer.OnVoidRaised -= EndHighlight;
    }

    private async void StartTimer()
    {
        try
        {
            await timer.Resume(destroyCancellationToken);
            EndHighlight();
        }
        catch (System.OperationCanceledException)
        {
        }
    }

    private void EndHighlight()
    {
        foreach (var cts in stopHighlightCtsDict)
        {
            cts.Value?.Cancel();
        }
    }

    private void HighlightHands(QuestBase quest)
    {
        if (quest.IsAllTarget)
        {
            foreach (var pos in HandTypeUtil.HandPosTypes)
            {
                HighlightHand(pos);
            }
        }
        else
        {
            HighlightHand(quest.TargetHandPos);
        }
    }

    private void HighlightHand(HandPosType posType)
    {
        stopHighlightCtsDict[posType] = new CancellationTokenSource();

        ImageHighlighter.Highlight(
            handPosImageDict[posType],
            highlightColor,
            highlightDuration,
            stopHighlightCtsDict[posType].Token
        ).Forget();
    }
}
