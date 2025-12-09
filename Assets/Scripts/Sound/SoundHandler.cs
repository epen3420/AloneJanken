using Cysharp.Threading.Tasks;
using SoundSystem;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    // [SerializeField]
    // private QuestEventChannelSO startRound;
    // [SerializeField]
    // private BoolEventChannelSO endJanken;
    [SerializeField]
    private VoidEventChannelSO changeTimeEvent;
    [SerializeField]
    private VoidEventChannelSO startRound;
    [SerializeField]
    private VoidEventChannelSO endGame;


    private void OnEnable()
    {
        startRound.OnVoidRaised += StartRoundSound;
        endGame.OnVoidRaised += StopBgm;
        changeTimeEvent.OnVoidRaised += ChangeTimeSound;
    }

    private void OnDisable()
    {
        startRound.OnVoidRaised -= StartRoundSound;
        endGame.OnVoidRaised -= StopBgm;
        changeTimeEvent.OnVoidRaised -= ChangeTimeSound;
    }

    private void StartRoundSound()
    {
        SoundPlayer.Instance.StopBgm();
        SoundPlayer.Instance.PlayBgm("janken").Forget();
        SoundPlayer.Instance.PlaySe("start_beep");
    }

    private void ChangeTimeSound()
    {
        SoundPlayer.Instance.PlaySe("beep");
    }

    private void StopBgm()
    {
        SoundPlayer.Instance.StopBgm();
    }
}
