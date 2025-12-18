using UnityEngine.Events;

public interface IInputDetectable
{
    void Enable();
    void Disable();

    public event UnityAction<Hand> OnInputHand;
}
