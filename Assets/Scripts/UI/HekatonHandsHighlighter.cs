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
    private CancellationTokenSource stopHighlightCts;


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
        await timer.Resume(destroyCancellationToken);
        stopHighlightCts?.Cancel();
    }

    private void EndHighlight()
    {
        stopHighlightCts?.Cancel();
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
        stopHighlightCts = new CancellationTokenSource();

        ImageHighlighter.Highlight(
            handPosImageDict[posType],
            highlightColor,
            highlightDuration,
            stopHighlightCts.Token
        ).Forget();
    }
}
