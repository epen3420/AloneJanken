using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HekatonHandsHighlighter : MonoBehaviour
{
    [SerializeField]
    private QuestEventChannelSO startRound;
    [SerializeField]
    private VoidEventChannelSO endJanken;
    [SerializeField]
    private float highlightDuration = 0.5f;
    [SerializeField]
    private HandPosImageMaps[] handPosImageMaps;

    [System.Serializable]
    private struct HandPosImageMaps
    {
        public HandPosType posType;
        public Image imageForHighlight;
    }

    private Dictionary<HandPosType, Image> handPosImageDict = new Dictionary<HandPosType, Image>();
    private Dictionary<HandPosType, CancellationTokenSource> stopHighlightCtsDict;


    private void Awake()
    {
        foreach (var map in handPosImageMaps)
        {
            handPosImageDict.Add(map.posType, map.imageForHighlight);
        }
    }

    private void OnEnable()
    {
        stopHighlightCtsDict = new Dictionary<HandPosType, CancellationTokenSource>();
        startRound.OnRaised += HighlightHands;

        endJanken.OnVoidRaised += EndHighlight;
    }

    private void OnDisable()
    {
        startRound.OnRaised -= HighlightHands;

        endJanken.OnVoidRaised -= EndHighlight;
    }

    private void EndHighlight()
    {
        foreach (var cts in stopHighlightCtsDict)
        {
            cts.Value?.Cancel();
            var image = handPosImageDict[cts.Key];
            image.gameObject.SetActive(false);

            var color = image.color;
            color.a = 1.0f;
            image.color = color;
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
        if (!handPosImageDict.TryGetValue(posType, out var imageForHighlight))
            return;

        var cts = new CancellationTokenSource();
        stopHighlightCtsDict[posType] = cts;

        imageForHighlight.gameObject.SetActive(true);
        imageForHighlight.DOFade(0, highlightDuration)
            .SetEase(Ease.Flash)
            .SetLoops(-1, LoopType.Yoyo)
            .ToUniTask(cancellationToken: cts.Token)
            .Forget();
    }
}
