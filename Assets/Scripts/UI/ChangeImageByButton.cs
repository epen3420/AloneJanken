using UnityEngine;
using UnityEngine.UI;

public class ChangeImageByButton : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private Button previousButton;
    [SerializeField]
    private Image image;

    private int currentIndex = 0;

    private void OnEnable()
    {
        nextButton.onClick.AddListener(OnNextButtonClicked);
        previousButton.onClick.AddListener(OnPreviousButtonClicked);

        UpdateImageAndButtons(currentIndex);
    }

    private void OnDisable()
    {
        nextButton.onClick.RemoveListener(OnNextButtonClicked);
        previousButton.onClick.RemoveListener(OnPreviousButtonClicked);
    }

    private void OnNextButtonClicked()
    {
        if (currentIndex < sprites.Length - 1)
        {
            currentIndex++;
            UpdateImageAndButtons(currentIndex);
        }
    }

    private void OnPreviousButtonClicked()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateImageAndButtons(currentIndex);
        }
    }

    private void UpdateImageAndButtons(int newIndex)
    {
        if (sprites != null && sprites.Length > 0 && newIndex >= 0 && newIndex < sprites.Length)
        {
            image.sprite = sprites[newIndex];
        }

        previousButton.interactable = newIndex > 0;
        nextButton.interactable = newIndex < sprites.Length - 1;
    }
}
