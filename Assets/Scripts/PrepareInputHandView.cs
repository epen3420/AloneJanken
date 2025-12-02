using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PrepareInputHandView : MonoBehaviour
{
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


    private void Start()
    {
        ResetText();
    }

    private void OnEnable()
    {
        inputEvent.OnRaised += SetView;
        endRound.OnRaised += ResetText;
    }

    private void OnDisable()
    {
        inputEvent.OnRaised -= SetView;
        endRound.OnRaised -= ResetText;
    }

    private void SetView(IEnumerable<Hand> inputHands)
    {
        var leftUpInput = inputHands.FirstOrDefault(h => h.pair.OwnerPos == HandPosType.LeftUp);
        var leftDownInput = inputHands.FirstOrDefault(h => h.pair.OwnerPos == HandPosType.LeftDown);
        var rightUpInput = inputHands.FirstOrDefault(h => h.pair.OwnerPos == HandPosType.RightUp);
        var rightDownInput = inputHands.FirstOrDefault(h => h.pair.OwnerPos == HandPosType.RightDown);

        leftUpText.SetText(leftUpInput?.pair.HandType.ToString());
        leftDownText.SetText(leftDownInput?.pair.HandType.ToString());
        rightUpText.SetText(rightUpInput?.pair.HandType.ToString());
        rightDownText.SetText(rightDownInput?.pair.HandType.ToString());
    }

    private void ResetText(bool _ = false)
    {
        leftUpText.SetText("");
        leftDownText.SetText("");
        rightUpText.SetText("");
        rightDownText.SetText("");
    }
}
