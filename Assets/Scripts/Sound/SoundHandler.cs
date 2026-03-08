using SoundSystem;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannelSO startRound;
    [SerializeField]
    private VoidEventChannelSO endGame;


    private void OnEnable()
    {
        startRound.OnVoidRaised += StartRoundSound;
        endGame.OnVoidRaised += StopBgm;
    }

    private void OnDisable()
    {
        startRound.OnVoidRaised -= StartRoundSound;
        endGame.OnVoidRaised -= StopBgm;
    }

    private void StartRoundSound()
    {
        SoundPlayer.Instance.StopBgm();
        SoundPlayer.Instance.PlaySe("start_beep");
    }


    private void StopBgm()
    {
        SoundPlayer.Instance.StopBgm();
    }
}
