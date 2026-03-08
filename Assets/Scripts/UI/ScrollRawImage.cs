using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ScrollRawImage : MonoBehaviour
{
    private RawImage rawImage;

    [SerializeField]
    private Vector2 direction;
    [SerializeField]
    private float speed;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }

    private void LateUpdate()
    {
        if (rawImage == null)
            rawImage = GetComponent<RawImage>();

        Rect uvRect = rawImage.uvRect;

        uvRect.x += -direction.x * speed * Time.deltaTime;
        uvRect.y += -direction.y * speed * Time.deltaTime;

        rawImage.uvRect = uvRect;
    }
}
