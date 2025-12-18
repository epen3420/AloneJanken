using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class KeyInputDetector : MonoBehaviour, IInputDetectable, System.IDisposable
{
    public event UnityAction<Hand> OnInputHand;

    private GameInputActions inputActions;
    // InputActionをキーにしてctx.actionで値を取れるようにしている
    private Dictionary<InputAction, Hand> actionMap;


    public KeyInputDetector(
        IEnumerable<HandPosType> useableHandPos)
    {
        inputActions = new GameInputActions();
        actionMap = new Dictionary<InputAction, Hand>();

        foreach (var posType in useableHandPos)
        {
            AddTargetHandPosActions(posType);
        }

        RegisterEvents();
    }

    private void AddTargetHandPosActions(HandPosType posType)
    {
        switch (posType)
        {
            case HandPosType.LeftUp:
                AddActionMap(inputActions.Janken.LeftUpRock, HandPosType.LeftUp, HandType.Rock);
                AddActionMap(inputActions.Janken.LeftUpScissors, HandPosType.LeftUp, HandType.Scissors);
                AddActionMap(inputActions.Janken.LeftUpPaper, HandPosType.LeftUp, HandType.Paper);
                break;
            case HandPosType.LeftDown:
                AddActionMap(inputActions.Janken.LeftDownRock, HandPosType.LeftDown, HandType.Rock);
                AddActionMap(inputActions.Janken.LeftDownScissors, HandPosType.LeftDown, HandType.Scissors);
                AddActionMap(inputActions.Janken.LeftDownPaper, HandPosType.LeftDown, HandType.Paper);
                break;
            case HandPosType.RightUp:
                // --- RightUp (右上) ---
                AddActionMap(inputActions.Janken.RightUpRock, HandPosType.RightUp, HandType.Rock);
                AddActionMap(inputActions.Janken.RightUpScissors, HandPosType.RightUp, HandType.Scissors);
                AddActionMap(inputActions.Janken.RightUpPaper, HandPosType.RightUp, HandType.Paper);
                break;
            case HandPosType.RightDown:
                // --- RightDown (右下) ---
                AddActionMap(inputActions.Janken.RightDownRock, HandPosType.RightDown, HandType.Rock);
                AddActionMap(inputActions.Janken.RightDownScissors, HandPosType.RightDown, HandType.Scissors);
                AddActionMap(inputActions.Janken.RightDownPaper, HandPosType.RightDown, HandType.Paper);
                break;
        }
    }

    /// <summary>
    /// actionMapに値を追加するためのヘルパー
    /// </summary>
    /// <param name="action"></param>
    /// <param name="pos"></param>
    /// <param name="type"></param>
    private void AddActionMap(InputAction action, HandPosType pos, HandType type)
    {
        var handPair = new Hand(type, pos);
        actionMap.Add(action, handPair);
    }

    private void RegisterEvents()
    {
        foreach (var action in actionMap.Keys)
        {
            action.performed += OnHandInput;
        }
    }

    private void UnRegisterEvents()
    {
        foreach (var action in actionMap.Keys)
        {
            action.performed -= OnHandInput;
        }
    }
    public void Dispose()
    {
        inputActions?.Disable();
        inputActions?.Dispose();

        inputActions = null;

        UnRegisterEvents();
    }

    private void OnHandInput(InputAction.CallbackContext ctx)
    {
        if (!actionMap.TryGetValue(ctx.action, out var value)) return;

        OnInputHand.Invoke(value);
    }

    public void Enable()
    {
        inputActions.Enable();

        Debug.Log("入力受付を開始");
    }

    public void Disable()
    {
        inputActions?.Disable();

        Debug.Log("入力受付を終了");
    }
}
