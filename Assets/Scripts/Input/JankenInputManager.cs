using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの入力を管理し、イベントを発行するクラス
/// </summary>
public class JankenInputManager : MonoBehaviour
{
    [Header("Event Channels")]
    [SerializeField] private HandsEventChannelSO inputHandsEvent;
    [SerializeField] private VoidEventChannelSO startRoundEvent;
    [SerializeField] private VoidEventChannelSO endJankenEvent;

    [Header("Configuration")]
    [SerializeField] private bool useRightHand = true;

    private GameInputActions gameInputActions;
    private Dictionary<InputAction, Hand> inputActionToHandMap;
    private List<Hand> currentInputHands = new List<Hand>();
    private bool isEnabled = false;

    private void Awake()
    {
        gameInputActions = new GameInputActions();
        inputActionToHandMap = new Dictionary<InputAction, Hand>();

        // --- Left Hand Registration ---
        RegisterInputAction(gameInputActions.Janken.LeftUpRock, HandPosType.LeftUp, HandType.Rock);
        RegisterInputAction(gameInputActions.Janken.LeftUpScissors, HandPosType.LeftUp, HandType.Scissors);
        RegisterInputAction(gameInputActions.Janken.LeftUpPaper, HandPosType.LeftUp, HandType.Paper);

        RegisterInputAction(gameInputActions.Janken.LeftDownRock, HandPosType.LeftDown, HandType.Rock);
        RegisterInputAction(gameInputActions.Janken.LeftDownScissors, HandPosType.LeftDown, HandType.Scissors);
        RegisterInputAction(gameInputActions.Janken.LeftDownPaper, HandPosType.LeftDown, HandType.Paper);

        // check if right hand is allowed
        if (IsRightHandAllowed())
        {
            // --- Right Hand Registration ---
            RegisterInputAction(gameInputActions.Janken.RightUpRock, HandPosType.RightUp, HandType.Rock);
            RegisterInputAction(gameInputActions.Janken.RightUpScissors, HandPosType.RightUp, HandType.Scissors);
            RegisterInputAction(gameInputActions.Janken.RightUpPaper, HandPosType.RightUp, HandType.Paper);

            RegisterInputAction(gameInputActions.Janken.RightDownRock, HandPosType.RightDown, HandType.Rock);
            RegisterInputAction(gameInputActions.Janken.RightDownScissors, HandPosType.RightDown, HandType.Scissors);
            RegisterInputAction(gameInputActions.Janken.RightDownPaper, HandPosType.RightDown, HandType.Paper);
        }
    }

    private bool IsRightHandAllowed()
    {
        // TODO: Remove dependency on SceneController by configuring 'useRightHand' in the inspector for each scene.
        // For now, adhere to legacy logic for Tutorial scene equality.
        if (SceneController.CurrentSceneName == "Tutorial") return false;

        return useRightHand;
    }

    private void RegisterInputAction(InputAction action, HandPosType pos, HandType type)
    {
        var hand = new Hand(type, pos);
        inputActionToHandMap.Add(action, hand);
    }

    private void OnEnable()
    {
        foreach (var action in inputActionToHandMap.Keys)
        {
            action.performed += OnHandInputPerformed;
        }

        startRoundEvent.OnVoidRaised += EnableInput;
        endJankenEvent.OnVoidRaised += DisableInput;
    }

    private void OnDisable()
    {
        foreach (var action in inputActionToHandMap.Keys)
        {
            action.performed -= OnHandInputPerformed;
        }

        startRoundEvent.OnVoidRaised -= EnableInput;
        endJankenEvent.OnVoidRaised -= DisableInput;
    }

    private void OnDestroy()
    {
        gameInputActions?.Disable();
        gameInputActions?.Dispose();
        gameInputActions = null;
    }

    private void OnHandInputPerformed(InputAction.CallbackContext ctx)
    {
        if (!isEnabled) return;
        if (!inputActionToHandMap.TryGetValue(ctx.action, out var hand)) return;

        UpdateInputHands(hand);
    }

    private void UpdateInputHands(Hand newHand)
    {
        // Replace existing hand for the same position, or add new
        int index = currentInputHands.FindIndex(h => h.Pos == newHand.Pos);

        if (index != -1)
        {
            currentInputHands[index] = newHand;
        }
        else
        {
            currentInputHands.Add(newHand);
        }

        inputHandsEvent.Raise(currentInputHands);
    }

    public void EnableInput()
    {
        isEnabled = true;
        currentInputHands.Clear();
        gameInputActions.Enable();

        Debug.Log("入力受付を開始");
    }

    public void DisableInput()
    {
        isEnabled = false;
        gameInputActions?.Disable();

        Debug.Log("入力受付を終了");
    }
}
