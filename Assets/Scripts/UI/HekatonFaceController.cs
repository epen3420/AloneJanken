using UnityEngine;
using UnityEngine.UI;

public class HekatonFaceController : MonoBehaviour
{
    [System.Serializable]
    private struct CountFaceMap
    {
        public int count;
        public Sprite face;
    }

    [SerializeField]
    private Image image;
    [SerializeField]
    private CountFaceMap[] countFaceMap;
    [SerializeField]
    private Sprite[] missCountFaceMaps;
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

    private void Miss(bool isWin)
    {

        if (isWin)
        {
            foreach (var map in countFaceMap)
            {
                if (map.count == continuousIndex)
                {
                    image.sprite = map.face;
                }
            }
            continuousIndex++;
        }
        else
        {
            continuousIndex = 0;
            image.sprite = missCountFaceMaps[missCountIndex++];

            if (missCountIndex >= missCountFaceMaps.Length)
            {
                missCountIndex = 0;
            }
        }
    }
}
