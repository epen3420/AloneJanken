using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class JankenInputManager : MonoBehaviour
{
    private GameInputActions inputActions;
    // InputActionをキーにしてctx.actionで値を取れるようにしている
    private Dictionary<InputAction, (HandPosType, HandType)> actionMap;
    private HashSet<(HandPosType, HandType)> currentInputHands = new HashSet<(HandPosType, HandType)>();
    private bool isEnable = false;

    public IEnumerable<Hand> CurrentInputHands => currentInputHands.Select(pair => new Hand(pair.Item2, pair.Item1));

    private void Awake()
    {
        inputActions = new GameInputActions();
        actionMap = new Dictionary<InputAction, (HandPosType, HandType)>();

        // --- LeftUp (左上) ---
        AddActionMap(inputActions.Janken.LeftUpRock, HandPosType.LeftUp, HandType.Rock);
        AddActionMap(inputActions.Janken.LeftUpScissors, HandPosType.LeftUp, HandType.Scissors);
        AddActionMap(inputActions.Janken.LeftUpPaper, HandPosType.LeftUp, HandType.Paper);

        // --- LeftDown (左下) ---
        AddActionMap(inputActions.Janken.LeftDownRock, HandPosType.LeftDown, HandType.Rock);
        AddActionMap(inputActions.Janken.LeftDownScissors, HandPosType.LeftDown, HandType.Scissors);
        AddActionMap(inputActions.Janken.LeftDownPaper, HandPosType.LeftDown, HandType.Paper);

        // --- RightUp (右上) ---
        AddActionMap(inputActions.Janken.RightUpRock, HandPosType.RightUp, HandType.Rock);
        AddActionMap(inputActions.Janken.RightUpScissors, HandPosType.RightUp, HandType.Scissors);
        AddActionMap(inputActions.Janken.RightUpPaper, HandPosType.RightUp, HandType.Paper);

        // --- RightDown (右下) ---
        AddActionMap(inputActions.Janken.RightDownRock, HandPosType.RightDown, HandType.Rock);
        AddActionMap(inputActions.Janken.RightDownScissors, HandPosType.RightDown, HandType.Scissors);
        AddActionMap(inputActions.Janken.RightDownPaper, HandPosType.RightDown, HandType.Paper);
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
        foreach (var action in actionMap.Keys)
        {
            action.performed += OnHandInput;
            action.canceled += OnHandInput;
        }
    }

    private void OnDisable()
    {
        foreach (var action in actionMap.Keys)
        {
            action.performed -= OnHandInput;
            action.canceled -= OnHandInput;
        }
    }

    private void OnDestroy()
    {
        inputActions?.Disable();
        inputActions?.Dispose();

        inputActions = null;
    }

    private void OnHandInput(InputAction.CallbackContext ctx)
    {
        if (!isEnable) return;
        if (actionMap.TryGetValue(ctx.action, out var value))
        {
            if (ctx.canceled)
            {
                Debug.Log(value);
                currentInputHands.Remove(value);
            }
            else
            {
                Debug.Log(value);
                currentInputHands.Add(value);
            }
        }
    }

    public void Enable()
    {
        isEnable = true;
        currentInputHands.Clear();
        inputActions.Enable();

        Debug.Log("入力受付を開始");
    }

    public void Disable()
    {
        isEnable = false;
        inputActions?.Disable();

        Debug.Log("入力受付を終了");
    }
}
