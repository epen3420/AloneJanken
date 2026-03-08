using SoundSystem;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField]
    private BoolEventChannelSO endJanken;
    [SerializeField]
    private VoidEventChannelSO endGame;

    private const string JANKEN_WIN_SE_NAME = "win_janken";
    private const string JANKEN_LOOSE_SE_NAME = "loose_janken";


    private void OnEnable()
    {
        endJanken.OnRaised += JudgeSE;
        endGame.OnVoidRaised += StopBgm;
    }

    private void OnDisable()
    {
        endJanken.OnRaised -= JudgeSE;
        endGame.OnVoidRaised -= StopBgm;
    }

    private void JudgeSE(bool isWin)
    {
        string seName = isWin ? JANKEN_WIN_SE_NAME : JANKEN_LOOSE_SE_NAME;
        SoundPlayer.Instance.PlaySe(seName);
    }

    private void StopBgm()
    {
        SoundPlayer.Instance.StopBgm();
    }
}
