using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderController : MonoBehaviour
{
    private Slider slider;
    private CancellationTokenSource sliderAnimationCts;


    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    /// <summary>
    /// スライダーの表示値を更新します。
    /// 0.0～1.0 の範囲で正規化された値を引数に渡してください
    /// </summary>
    /// <param name="value"></param>
    public void UpdateSlider(float value)
    {
        if (value < 0.0f || 1.0f < value)
        {
            value = Mathf.Clamp(value, 0.0f, 1.0f);
            Debug.LogWarning($"[{this.name}] {this.gameObject.name}: Received out-of-range value {value}. Clamping to [0.0f, 1.0f].");
        }

        slider.value = value;
    }

    public async UniTask UpdateSliderWithEasing(
        float to,
        float duration,
        Ease ease = Ease.Linear)
    {
        if (slider == null)
        {
            Debug.LogWarning($"{gameObject.name}: Slider is null");
        }

        if (to < 0.0f || 1.0f < to)
        {
            to = Mathf.Clamp(to, 0.0f, 1.0f);
            Debug.LogWarning($"[{this.name}] {this.gameObject.name}: Received out-of-range value {to}. Clamping to [0.0f, 1.0f].");
        }

        // アニメーションしてたらキャンセル
        sliderAnimationCts?.Cancel();
        sliderAnimationCts = new CancellationTokenSource();

        var ctn = sliderAnimationCts.Token;

        await slider.DOValue(to, duration)
            .SetEase(ease)
            .ToUniTask(cancellationToken: ctn);

        if (slider != null && !ctn.IsCancellationRequested)
            slider.value = to;
    }

    private void OnDestroy()
    {
        sliderAnimationCts?.Cancel();
        sliderAnimationCts?.Dispose();
        sliderAnimationCts = null;
    }
}
