using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class JankenInputManager : MonoBehaviour
{
    private GameInputActions inputActions;
    // InputActionをキーにしてctx.actionで値を取れるようにしている
    private Dictionary<InputAction, Hand.Pair> actionMap;
    private HashSet<Hand.Pair> currentInputHands = new HashSet<Hand.Pair>();
    private bool isEnable = false;

    public IEnumerable<Hand> CurrentInputHands => currentInputHands.Select(pair => new Hand(pair.HandType, pair.OwnerPos));

    private void Awake()
    {
        inputActions = new GameInputActions();
        actionMap = new Dictionary<InputAction, Hand.Pair>();

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
        var handPair = new Hand.Pair
        {
            HandType = type,
            OwnerPos = pos,
        };
        actionMap.Add(action, handPair);
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
