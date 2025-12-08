using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LifeViewer : MonoBehaviour
{
    [SerializeField]
    private Sprite lifeSprite;
    [SerializeField]
    private Sprite hurtLifeSprite;

    private Image image;


    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetSprite(bool isLife)
    {
        if (isLife)
        {
            image.sprite = lifeSprite;
        }
        else
        {
            image.sprite = hurtLifeSprite;
        }
    }
}
