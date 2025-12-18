using System.Collections.Generic;
using UnityEngine;

public class JankenInputManager : MonoBehaviour
{
    [SerializeField]
    private HandsEventChannelSO onInputHands;
    [SerializeField]
    private IInputDetectable[] inputHandlers;

    private List<Hand> currentInputHands;


    public void StartInput(IEnumerable<HandPosType> useableHandPos)
    {
        foreach (var inputHandler in inputHandlers)
        {
            inputHandler.OnInputHand += RaiseCurrentInputHands;
        }
    }

    public IEnumerable<Hand> GetCurrentInputHands() => currentInputHands;

    public void EndInput()
    {
        foreach (var inputHandler in inputHandlers)
        {
            inputHandler?.Disable();
        }
    }

    private void RaiseCurrentInputHands(Hand inputHand)
    {
        int index = currentInputHands.FindIndex(h => h.pair.OwnerPos == inputHand.pair.OwnerPos);

        if (index != -1)
        {
            currentInputHands[index] = inputHand;
        }
        else
        {
            currentInputHands.Add(inputHand);
        }

        onInputHands.Raise(currentInputHands);
    }
}
