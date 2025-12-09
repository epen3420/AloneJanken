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
    private VoidEventChannelSO endBeats;
    [SerializeField]
    private VoidEventChannelSO endGame;
    [SerializeField]
    private string[] jankenVoices;

    private int currentVoiceIndex = 0;


    private void OnEnable()
    {
        startRound.OnVoidRaised += StartRoundSound;
        endGame.OnVoidRaised += StopBgm;
        endBeats.OnVoidRaised += ResetIndex;
        endBeats.OnVoidRaised += ResetIndex;
        changeTimeEvent.OnVoidRaised += ChangeTimeSound;
    }

    private void OnDisable()
    {
        startRound.OnVoidRaised -= StartRoundSound;
        endGame.OnVoidRaised -= StopBgm;
        endBeats.OnVoidRaised -= ResetIndex;
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
        SoundPlayer.Instance.PlayVoice(jankenVoices[currentVoiceIndex++]);
    }

    private void StopBgm()
    {
        SoundPlayer.Instance.StopBgm();
    }

    private void ResetIndex()
    {
        currentVoiceIndex = 0;
    }
}
