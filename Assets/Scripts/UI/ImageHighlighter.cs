using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class ImageHighlighter
{
    public static async UniTaskVoid Highlight(
        Image image,
        Color highlightColor,
        float duration,
        CancellationToken ctn)
    {
        await image.DOColor(highlightColor, duration)
            .SetEase(Ease.Flash)
            .SetLoops(-1, LoopType.Yoyo)
            .ToUniTask(cancellationToken: ctn);
    }
}
