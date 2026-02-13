using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyInputProvider : BaseInputProvider
{
    [Header("Configuration")]
    [SerializeField] private bool useUpHand = true;

    private GameInputActions gameInputActions;
    private Dictionary<InputAction, Hand> inputActionToHandMap;

    private void Awake()
    {
        InitializeKeyInputs();
    }

    private void InitializeKeyInputs()
    {
        gameInputActions = new GameInputActions();
        inputActionToHandMap = new Dictionary<InputAction, Hand>();

        // --- Down Hand Registration ---
        RegisterInputAction(gameInputActions.Janken.LeftDownRock, HandPosType.LeftDown, HandType.Rock);
        RegisterInputAction(gameInputActions.Janken.LeftDownScissors, HandPosType.LeftDown, HandType.Scissors);
        RegisterInputAction(gameInputActions.Janken.LeftDownPaper, HandPosType.LeftDown, HandType.Paper);
        RegisterInputAction(gameInputActions.Janken.RightDownRock, HandPosType.RightDown, HandType.Rock);
        RegisterInputAction(gameInputActions.Janken.RightDownScissors, HandPosType.RightDown, HandType.Scissors);
        RegisterInputAction(gameInputActions.Janken.RightDownPaper, HandPosType.RightDown, HandType.Paper);

        if (useUpHand)
        {
            // --- Up Hand Registration ---
            RegisterInputAction(gameInputActions.Janken.RightUpRock, HandPosType.RightUp, HandType.Rock);
            RegisterInputAction(gameInputActions.Janken.RightUpScissors, HandPosType.RightUp, HandType.Scissors);
            RegisterInputAction(gameInputActions.Janken.RightUpPaper, HandPosType.RightUp, HandType.Paper);

            RegisterInputAction(gameInputActions.Janken.LeftUpRock, HandPosType.LeftUp, HandType.Rock);
            RegisterInputAction(gameInputActions.Janken.LeftUpScissors, HandPosType.LeftUp, HandType.Scissors);
            RegisterInputAction(gameInputActions.Janken.LeftUpPaper, HandPosType.LeftUp, HandType.Paper);
        }
    }

    private void RegisterInputAction(InputAction action, HandPosType pos, HandType type)
    {
        var hand = new Hand(type, pos);
        inputActionToHandMap.Add(action, hand);
    }

    private void OnDestroy()
    {
        gameInputActions?.Disable();
        gameInputActions?.Dispose();
        gameInputActions = null;
    }

    private void OnHandInputPerformed(InputAction.CallbackContext ctx)
    {
        if (!inputActionToHandMap.TryGetValue(ctx.action, out var hand)) return;
        RaiseInputDetected(hand);
    }

    public override void EnableInput()
    {
        gameInputActions?.Enable();
        foreach (var action in inputActionToHandMap.Keys)
        {
            action.performed += OnHandInputPerformed;
        }
    }

    public override void DisableInput()
    {
        foreach (var action in inputActionToHandMap.Keys)
        {
            action.performed -= OnHandInputPerformed;
        }
        gameInputActions?.Disable();
    }
}
