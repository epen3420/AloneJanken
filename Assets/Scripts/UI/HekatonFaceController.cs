using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HekatonFaceController : MonoBehaviour
{
    [System.Serializable]
    private struct CountFaceMap
    {
        public int count;
        public Sprite face;
        public bool needEye;
    }

    [SerializeField]
    private Image faceImage;
    [SerializeField]
    private Image eyeImage;
    [SerializeField]
    private CountFaceMap[] countFaceMaps;
    [SerializeField]
    private CountFaceMap[] missCountFaceMaps;
    [SerializeField]
    private BoolEventChannelSO endJanken;
    [SerializeField]
    private IntEventChannelSO continuousCount;

    private int missCountIndex = 0;
    private int continuousIndex = 0;


    private void OnEnable()
    {
        endJanken.OnRaised += Miss;
    }

    private void OnDisable()
    {
        endJanken.OnRaised -= Miss;
    }

    private void Miss(bool isWin)
    {
        if (isWin)
        {
            continuousIndex++;
            if (!countFaceMaps.Any(map => map.count == continuousIndex))
            {
                faceImage.sprite = countFaceMaps[0].face;
                return;
            }

            foreach (var map in countFaceMaps)
            {
                eyeImage.enabled = map.needEye;
                if (map.count == continuousIndex)
                {
                    faceImage.sprite = map.face;
                    break;
                }
            }

            if (continuousIndex >= countFaceMaps.Length)
            {
                continuousIndex = 0;
            }
        }
        else
        {
            continuousIndex = 0;
            missCountIndex++;

            if (!missCountFaceMaps.Any(map => map.count == missCountIndex))
            {
                faceImage.sprite = countFaceMaps[0].face;
                return;
            }

            foreach (var map in missCountFaceMaps)
            {
                eyeImage.enabled = map.needEye;
                if (map.count == missCountIndex)
                {
                    faceImage.sprite = map.face;
                    break;
                }
            }

            if (missCountIndex >= missCountFaceMaps.Length)
            {
                missCountIndex = 0;
            }
        }
    }
}
