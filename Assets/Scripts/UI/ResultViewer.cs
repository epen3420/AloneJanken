using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ResultViewer : MonoBehaviour
{
    private TMP_Text text;


    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public async UniTask CountScore(int targetScore, float duration = 1.0f)
    {
        int currentScore = 0;
        await DOTween.To(
            () => currentScore,
            x =>
            {
                currentScore = x;
                text.SetText($"{currentScore}");
            },
            targetScore,
            duration);
    }
}
