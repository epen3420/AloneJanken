using UnityEngine;
using UnityEngine.Events;

public abstract class BaseInputProvider : MonoBehaviour
{
    public event UnityAction<Hand> OnInputDetected;

    protected void RaiseInputDetected(Hand hand)
    {
        OnInputDetected?.Invoke(hand);
    }

    public abstract void EnableInput();
    public abstract void DisableInput();
}
