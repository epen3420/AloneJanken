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
    private IntEventChannelSO changeBeats;

    private Animator[] allAnimators => upHandAnimators.Concat(downHandAnimators).ToArray();


    private void OnEnable()
    {
        startNormalLevel.OnVoidRaised += AppearHands;
        changeBeats.OnRaised += JankenAnim;
    }

    private void OnDisable()
    {
        startNormalLevel.OnVoidRaised -= AppearHands;
        changeBeats.OnRaised -= JankenAnim;
    }

    private void AppearHands()
    {
        foreach (var animator in upHandAnimators)
        {
            animator.SetTrigger("Appear");
        }
    }

    private void JankenAnim(int count)
    {
        if (count != 0) return;

        foreach (var animator in allAnimators)
        {
            float speed = animator.GetNextAnimatorClipInfo(0)
                 .FirstOrDefault(state => state.clip.name == "Janken")
                 .clip.length / 8;
            animator.SetFloat("MotionTime", speed);
            animator.SetTrigger("Janken");
        }
    }
}
