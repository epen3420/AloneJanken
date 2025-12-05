using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreAddAnim : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 0.5f;
    [SerializeField]
    private float moveDistance = 0.5f;
    [SerializeField]
    private float moveDuration = 0.2f;
    [SerializeField]
    private float startFadeDelay = 0.1f;

    private TMP_Text text;


    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        StartAnim();
    }

    private void StartAnim()
    {
        transform.DOMoveY(transform.position.y + moveDistance, moveDuration)
            .SetEase(Ease.OutQuad);

        text.DOFade(0f, fadeDuration)
            .SetDelay(startFadeDelay)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }
}
