using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEventChannelSO", menuName = "Events/VoidEventChannelSO")]
public class VoidEventChannelSO : ScriptableObject
{
    public event UnityAction OnVoidRaised;


    public void Raise()
    {
        if (OnVoidRaised == null)
        {
            Debug.LogWarning($"{name} was raised but no listeners were found.");
            return;
        }

        OnVoidRaised.Invoke();
    }
}
