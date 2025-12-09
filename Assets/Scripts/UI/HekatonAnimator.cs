using System.Linq;
using UnityEngine;

public class HekatonAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator[] upHandAnimators;
    [SerializeField]
    private Animator[] downHandAnimators;
    [SerializeField]
    private VoidEventChannelSO startNormalLevel;
    [SerializeField]
    private VoidEventChannelSO startRound;

    private Animator[] allAnimators => upHandAnimators.Concat(downHandAnimators).ToArray();


    private void OnEnable()
    {
        startNormalLevel.OnVoidRaised += AppearHands;
        startRound.OnVoidRaised += JankenAnim;
    }

    private void OnDisable()
    {
        startNormalLevel.OnVoidRaised -= AppearHands;
        startRound.OnVoidRaised -= JankenAnim;
    }

    private void AppearHands()
    {
        foreach (var animator in upHandAnimators)
        {
            animator.SetTrigger("Appear");
        }
    }

    private void JankenAnim()
    {
        foreach (var animator in allAnimators)
        {
            animator.SetTrigger("Janken");
        }
    }
}
