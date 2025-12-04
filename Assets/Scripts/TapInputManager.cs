using UnityEngine;
using UnityEngine.Events;

public class TapInputManager : MonoBehaviour
{
    [System.Serializable]
    private struct PosTypeButtonMap
    {
        public HandPosType posType;
        public TapInputController tapInputController;
    }

    [SerializeField]
    private PosTypeButtonMap[] posTypeButtonMaps;
    [SerializeField]
    private JankenInputManager inputManager;

    private UnityAction<HandType>[] onClickActions;


    private void OnEnable()
    {
        onClickActions = new UnityAction<HandType>[posTypeButtonMaps.Length];
        for (int i = 0; i < posTypeButtonMaps.Length; i++)
        {
            int index = i;
            UnityAction<HandType> action = (hand) =>
                inputManager.ChangeHandInput(new Hand(hand, posTypeButtonMaps[index].posType));

            posTypeButtonMaps[index].tapInputController.OnClick += action;
            onClickActions[index] = action;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < posTypeButtonMaps.Length; i++)
        {
            posTypeButtonMaps[i].tapInputController.OnClick -= onClickActions[i];
        }
    }
}
