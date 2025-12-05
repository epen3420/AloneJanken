using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PrepareInputHandView : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannelSO startRound;
    [SerializeField]
    private HandsEventChannelSO inputEvent;
    [SerializeField]
    private BoolEventChannelSO endRound;
    [SerializeField]
    private TMP_Text leftUpText;
    [SerializeField]
    private TMP_Text leftDownText;
    [SerializeField]
    private TMP_Text rightUpText;
    [SerializeField]
    private TMP_Text rightDownText;


    private void OnEnable()
    {
        startRound.OnVoidRaised += ResetText;
        inputEvent.OnRaised += SetView;
    }

    private void OnDisable()
    {
        startRound.OnVoidRaised -= ResetText;
        inputEvent.OnRaised -= SetView;
    }

    private void SetView(IEnumerable<Hand> inputHands)
    {
        var leftUpInput = inputHands.FirstOrDefault(h => h.pair.OwnerPos == HandPosType.LeftUp);
        var leftDownInput = inputHands.FirstOrDefault(h => h.pair.OwnerPos == HandPosType.LeftDown);
        var rightUpInput = inputHands.FirstOrDefault(h => h.pair.OwnerPos == HandPosType.RightUp);
        var rightDownInput = inputHands.FirstOrDefault(h => h.pair.OwnerPos == HandPosType.RightDown);

        if (leftUpInput != null)
            leftUpText.SetText(leftUpInput?.pair.HandType.ToString());
        if (leftDownInput != null)
            leftDownText.SetText(leftDownInput?.pair.HandType.ToString());
        if (rightUpInput != null)
            rightUpText.SetText(rightUpInput?.pair.HandType.ToString());
        if (rightDownInput != null)
            rightDownText.SetText(rightDownInput?.pair.HandType.ToString());
    }

    private void ResetText()
    {
        leftUpText.SetText("");
        leftDownText.SetText("");
        rightUpText.SetText("");
        rightDownText.SetText("");
    }
}
