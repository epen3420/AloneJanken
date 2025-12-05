using SoundManagement;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    // [SerializeField]
    // private QuestEventChannelSO startRound;
    // [SerializeField]
    // private BoolEventChannelSO endRound;
    [SerializeField]
    private FloatEventChannelSO changeTimeEvent;

    private int passedTime = 0;

    private void OnEnable()
    {
        changeTimeEvent.OnRaised += ChangeTimeSound;
    }

    private void ChangeTimeSound(float time)
    {
        int currentTimeInt = (int)time;
        if (passedTime != currentTimeInt)
        {
            SoundPlayer.Instance.PlaySe("beep");

            passedTime = currentTimeInt;
        }
    }
}
