using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEventChannelSO", menuName = "Events/VoidEventChannelSO")]
public class VoidEventChannelSO : ScriptableObject
{
    public event UnityAction OnVoidRaised;


    public void Raise()
    {
        OnVoidRaised?.Invoke();
    }
}
