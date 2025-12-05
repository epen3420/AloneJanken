using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DecidedInputHandView : MonoBehaviour
{
    [System.Serializable]
    private struct HandTypeSpriteMap
    {
        public HandType handType;
        public Sprite sprite;
    }
    [System.Serializable]
    private struct HandPosImageMap
    {
        public HandPosType handPosType;
        public Image setImage;
    }

    [SerializeField]
    private VoidEventChannelSO startRound;
    [SerializeField]
    private HandsEventChannelSO endInput;
    [SerializeField]
    private HandTypeSpriteMap[] handTypeSpriteMaps;
    [SerializeField]
    private HandPosImageMap[] handPosImageMaps;

    private Dictionary<HandType, Sprite> handTypeSpriteDict = new Dictionary<HandType, Sprite>();
    private Dictionary<HandPosType, Image> handPairImageDict = new Dictionary<HandPosType, Image>();
    private IEnumerable<Hand> defaultHands;


    private void Awake()
    {
        defaultHands = HandTypeUtil.HandPosTypes.Select(pos => new Hand(HandType.Rock, pos));

        foreach (var handSpritePair in handTypeSpriteMaps)
        {
            if (!handTypeSpriteDict.TryAdd(handSpritePair.handType, handSpritePair.sprite))
            {
                throw new System.Exception($"[Duplicate handTypeSpriteDictionary key at {this.name}] already exist key: {handSpritePair.handType}");
            }

        }

        foreach (var handPosImagePair in handPosImageMaps)
        {
            if (!handPairImageDict.TryAdd(handPosImagePair.handPosType, handPosImagePair.setImage))
            {
                throw new System.Exception($"[Duplicate handPairImageDictionary key at {this.name}] already exist key: {handPosImagePair.handPosType}");
            }
        }
    }

    private void OnEnable()
    {
        startRound.OnVoidRaised += ResetView;
        endInput.OnRaised += SetView;
    }

    private void OnDisable()
    {
        startRound.OnVoidRaised -= ResetView;
        endInput.OnRaised -= SetView;
    }

    private void ResetView()
    {
        SetView(defaultHands);
    }

    private void SetView(IEnumerable<Hand> hands)
    {
        foreach (var hand in hands)
        {
            var sprite = handTypeSpriteDict[hand.pair.HandType];
            var image = handPairImageDict[hand.pair.OwnerPos];

            if (image.sprite != sprite)
            {
                image.sprite = sprite;
            }
        }
    }
}
