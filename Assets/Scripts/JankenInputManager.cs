using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JankenInputManager : MonoBehaviour
{
    private GameInputActions inputActions;
    // InputActionをキーにしてctx.actionで値を取れるようにしている
    private Dictionary<InputAction, (HandPosType, HandType)> actionMap;
    private HashSet<Hand> hands = new HashSet<Hand>();

    private void Awake()
    {
        inputActions = new GameInputActions();
        actionMap = new Dictionary<InputAction, (HandPosType, HandType)>();

        AddActionMap(inputActions.Janken.LeftDownPaper, HandPosType.LeftDown, HandType.Paper);
        AddActionMap(inputActions.Janken.LeftDownRock, HandPosType.LeftDown, HandType.Rock);
    }

    /// <summary>
    /// actionMapに値を追加するためのヘルパー
    /// </summary>
    /// <param name="action"></param>
    /// <param name="pos"></param>
    /// <param name="type"></param>
    private void AddActionMap(InputAction action, HandPosType pos, HandType type)
    {
        actionMap.Add(action, (pos, type));
    }

    private void OnEnable()
    {
        inputActions.Enable();

        foreach (var action in actionMap.Keys)
        {
            action.performed += OnHandInput;
        }
    }

    private void OnDisable()
    {
        foreach (var action in actionMap.Keys)
        {
            action.performed -= OnHandInput;
        }

        inputActions.Disable();
    }

    private void OnHandInput(InputAction.CallbackContext ctx)
    {
        if (actionMap.TryGetValue(ctx.action, out var value))
        {
            var (pos, type) = value;
            RegisterHand(pos, type);
        }
    }

    private void RegisterHand(HandPosType posType, HandType handType)
    {
        // ここに実際のロジック
        Debug.Log($"Input: {posType}, {handType}");
        // hands.Add(...);
    }
}
