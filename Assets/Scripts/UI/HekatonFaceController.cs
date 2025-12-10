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
    private Image image;
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
                image.sprite = countFaceMaps[0].face;
                return;
            }

            foreach (var map in countFaceMaps)
            {
                if (map.count == continuousIndex)
                {
                    image.sprite = map.face;
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
                image.sprite = countFaceMaps[0].face;
                return;
            }

            foreach (var map in missCountFaceMaps)
            {
                if (map.count == missCountIndex)
                {
                    image.sprite = map.face;
                }
            }

            if (missCountIndex >= missCountFaceMaps.Length)
            {
                missCountIndex = 0;
            }
        }
    }
}
