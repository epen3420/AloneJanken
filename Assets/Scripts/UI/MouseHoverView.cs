using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // これが必要です

[RequireComponent(typeof(Button))]
public class MouseHoverView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    [SerializeField] private float hoverScaleAmount = 1.1f; // ホバー時の拡大率
    [SerializeField] private float smoothSpeed = 10f;       // アニメーションの速度

    private Vector3 defaultScale;
    private Vector3 targetScale;

    private void Awake()
    {
        // 初期サイズを保存
        defaultScale = transform.localScale;
        targetScale = defaultScale;
    }

    private void Update()
    {
        // 毎フレーム滑らかにサイズを変化させる (線形補間)
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * smoothSpeed);
    }

    private void OnDisable()
    {
        targetScale = defaultScale;
        transform.localScale = defaultScale;
    }

    // マウスカーソルが乗ったときに呼ばれる
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = defaultScale * hoverScaleAmount;
    }

    // マウスカーソルが離れたときに呼ばれる
    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = defaultScale;
    }
}
