using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TapInputController : MonoBehaviour
{
    public event UnityAction<HandType> OnClick;

    private Button button;
    private int handTypeIndex;
    private readonly HandType[] handTypes = HandTypeUtil.HandTypes;


    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.interactable = true;
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClick);
        button.interactable = false;
        handTypeIndex = 0;
    }

    private void OnButtonClick()
    {
        OnClick?.Invoke(LoopHandType());
    }

    private HandType LoopHandType()
    {
        if (handTypeIndex >= HandTypeUtil.HandCount)
            handTypeIndex = 0;

        return handTypes[handTypeIndex++];
    }
}
