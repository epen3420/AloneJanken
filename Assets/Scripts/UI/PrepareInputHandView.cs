using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PrepareInputHandView : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannelSO startRound;
    [SerializeField]
    private HandsEventChannelSO inputEvent;
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
    private HandTypeSpriteMap[] handTypeSpriteMaps;
    [SerializeField]
    private HandPosImageMap[] handPosImageMaps;

    private Dictionary<HandType, Sprite> handTypeSpriteDict = new Dictionary<HandType, Sprite>();
    private Dictionary<HandPosType, Image> handPairImageDict = new Dictionary<HandPosType, Image>();


    private void Awake()
    {
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
        inputEvent.OnRaised += SetView;
    }

    private void OnDisable()
    {
        startRound.OnVoidRaised -= ResetView;
        inputEvent.OnRaised -= SetView;
    }

    private void ResetView()
    {
        foreach (var image in handPairImageDict)
        {
            image.Value.enabled = false;
            image.Value.sprite = null;
        }
    }

    private void SetView(IEnumerable<Hand> hands)
    {
        foreach (var hand in hands)
        {
            var sprite = handTypeSpriteDict[hand.pair.HandType];
            var image = handPairImageDict[hand.pair.OwnerPos];

            image.enabled = true;
            if (image.sprite != sprite)
            {
                image.sprite = sprite;
            }
        }
    }
}
