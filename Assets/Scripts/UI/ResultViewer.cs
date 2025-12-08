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

    public async UniTask CountScore(int score, float duration = 1.0f)
    {
        await DOTween.To(() => 0, x => text.SetText($"{x}"), score, duration);
    }
}
