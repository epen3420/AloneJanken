using SoundManagement;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    // [SerializeField]
    // private QuestEventChannelSO startRound;
    // [SerializeField]
    // private BoolEventChannelSO endRound;
    [SerializeField]
    private VoidEventChannelSO changeTimeEvent;
    [SerializeField]
    private VoidEventChannelSO startRound;


    private void OnEnable()
    {
        startRound.OnVoidRaised += StartRoundSound;
        changeTimeEvent.OnVoidRaised += ChangeTimeSound;
    }

    private void StartRoundSound()
    {
        SoundPlayer.Instance.PlaySe("start_beep");
    }

    private void ChangeTimeSound()
    {
        SoundPlayer.Instance.PlaySe("beep");
    }
}
