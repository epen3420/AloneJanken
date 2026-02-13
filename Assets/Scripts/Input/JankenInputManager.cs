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

    [Header("Input Providers")]
    [SerializeField] private List<BaseInputProvider> inputProviders;

    private List<Hand> currentInputHands = new List<Hand>();
    private bool isEnabled = false;

    private void Awake()
    {
        // Auto-detect providers if not manually assigned
        if (inputProviders == null || inputProviders.Count == 0)
        {
            inputProviders = new List<BaseInputProvider>(GetComponents<BaseInputProvider>());
            inputProviders.AddRange(GetComponentsInChildren<BaseInputProvider>());
        }
    }

    private void OnEnable()
    {
        SubscribeToProviders();

        startRoundEvent.OnVoidRaised += EnableInput;
        endJankenEvent.OnVoidRaised += DisableInput;
    }

    private void OnDisable()
    {
        UnsubscribeFromProviders();

        startRoundEvent.OnVoidRaised -= EnableInput;
        endJankenEvent.OnVoidRaised -= DisableInput;
    }

    private void SubscribeToProviders()
    {
        if (inputProviders == null) return;
        foreach (var provider in inputProviders)
        {
            if (provider != null)
                provider.OnInputDetected += UpdateInputHands;
        }
    }

    private void UnsubscribeFromProviders()
    {
        if (inputProviders == null) return;
        foreach (var provider in inputProviders)
        {
            if (provider != null)
                provider.OnInputDetected -= UpdateInputHands;
        }
    }

    private void UpdateInputHands(Hand newHand)
    {
        if (!isEnabled) return;

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

        if (inputProviders != null)
        {
            foreach (var provider in inputProviders)
            {
                provider?.EnableInput();
            }
        }

        Debug.Log("入力受付を開始");
    }

    public void DisableInput()
    {
        isEnabled = false;

        if (inputProviders != null)
        {
            foreach (var provider in inputProviders)
            {
                provider?.DisableInput();
            }
        }

        Debug.Log("入力受付を終了");
    }
}
