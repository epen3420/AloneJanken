using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class JankenInputManager : MonoBehaviour
{
    [SerializeField]
    private HandsEventChannelSO inputHandsEvent;
    [SerializeField]
    private VoidEventChannelSO startTimer;
    [SerializeField]
    private VoidEventChannelSO endTimer;

    private GameInputActions inputActions;
    // InputActionをキーにしてctx.actionで値を取れるようにしている
    private Dictionary<InputAction, Hand> actionMap;
    private List<Hand> currentInputHands = new List<Hand>();
    private bool isEnable = false;


    private void Awake()
    {
        inputActions = new GameInputActions();
        actionMap = new Dictionary<InputAction, Hand>();

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
        var handPair = new Hand(type, pos);
        actionMap.Add(action, handPair);
    }

    private void OnEnable()
    {
        foreach (var action in actionMap.Keys)
        {
            action.performed += OnHandInput;
        }

        startTimer.OnVoidRaised += Enable;
        endTimer.OnVoidRaised += Disable;
    }

    private void OnDisable()
    {
        foreach (var action in actionMap.Keys)
        {
            action.performed -= OnHandInput;
        }

        startTimer.OnVoidRaised -= Enable;
        endTimer.OnVoidRaised -= Disable;
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
        if (!actionMap.TryGetValue(ctx.action, out var value)) return;

        ChangeHandInput(value);
    }

    public void ChangeHandInput(Hand hand)
    {
        int index = currentInputHands.FindIndex(h => h.pair.OwnerPos == hand.pair.OwnerPos);

        if (index != -1)
        {
            currentInputHands[index] = hand;
        }
        else
        {
            currentInputHands.Add(hand);
        }

        inputHandsEvent.Raise(currentInputHands);
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
