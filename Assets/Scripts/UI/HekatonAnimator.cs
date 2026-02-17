using UnityEngine;

public class HekatonAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private VoidEventChannelSO startNormalLevel;


    private void OnEnable()
    {
        startNormalLevel.OnVoidRaised += AppearHands;
    }

    private void OnDisable()
    {
        startNormalLevel.OnVoidRaised -= AppearHands;
    }

    private void AppearHands()
    {
        animator.SetTrigger("Appear");
    }
}
