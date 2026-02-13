using UnityEngine;
using UnityEngine.Events;

public class UIInputProvider : BaseInputProvider
{
    [System.Serializable]
    public struct PosTypeButtonMap
    {
        public HandPosType posType;
        public TapInputController tapInputController;
    }

    [Header("UI Configuration")]
    [SerializeField] private PosTypeButtonMap[] posTypeButtonMaps;

    private UnityAction<HandType>[] onUiClickActions;

    public override void EnableInput()
    {
        if (posTypeButtonMaps == null) return;

        // Ensure actions array is initialized
        if (onUiClickActions == null || onUiClickActions.Length != posTypeButtonMaps.Length)
        {
            onUiClickActions = new UnityAction<HandType>[posTypeButtonMaps.Length];
        }

        for (int i = 0; i < posTypeButtonMaps.Length; i++)
        {
            var map = posTypeButtonMaps[i];
            if (map.tapInputController == null) continue;

            // Enable the button interaction
            map.tapInputController.enabled = true;

            // Avoid double subscription
            if (onUiClickActions[i] != null)
            {
                map.tapInputController.OnClick -= onUiClickActions[i];
            }

            // Create and subscribe action
            int index = i;
            UnityAction<HandType> action = (handType) =>
            {
                RaiseInputDetected(new Hand(handType, map.posType));
            };

            map.tapInputController.OnClick += action;
            onUiClickActions[index] = action;
        }
    }

    public override void DisableInput()
    {
        if (posTypeButtonMaps == null) return;

        for (int i = 0; i < posTypeButtonMaps.Length; i++)
        {
            var map = posTypeButtonMaps[i];
            if (map.tapInputController != null)
            {
                map.tapInputController.enabled = false;
                if (onUiClickActions != null && onUiClickActions[i] != null)
                {
                    map.tapInputController.OnClick -= onUiClickActions[i];
                    onUiClickActions[i] = null;
                }
            }
        }
    }
}
