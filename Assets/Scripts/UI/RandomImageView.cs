using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RandomImageView : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        SceneController.OnStartLoading += SetView;
    }

    private void OnDisable()
    {
        SceneController.OnStartLoading -= SetView;
    }

    private void SetView(LoadMethodType loadMethod)
    {
        if (sprites == null || sprites.Length == 0) return;

        bool isShow = loadMethod != LoadMethodType.Immediate;
        image.gameObject.SetActive(isShow);

        if (isShow)
        {
            image.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }
}
