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

    [Header("UI References")]
    [SerializeField] private Image faceImage;
    [SerializeField] private Image eyeImage;

    [Header("Settings")]
    [SerializeField] private CountFaceMap[] winFaceMaps;
    [SerializeField] private CountFaceMap[] loseFaceMaps;

    [Header("Events")]
    [SerializeField] private BoolEventChannelSO endJankenEvent;

    private int currentLoseCount = 0;
    private int currentWinCount = 0;

    private void OnEnable()
    {
        if (endJankenEvent != null)
            endJankenEvent.OnRaised += OnJankenEnd;
    }

    private void OnDisable()
    {
        if (endJankenEvent != null)
            endJankenEvent.OnRaised -= OnJankenEnd;
    }

    private void OnJankenEnd(bool isWin)
    {
        if (isWin)
        {
            HandleWin();
        }
        else
        {
            HandleLose();
        }
    }

    private void HandleWin()
    {
        currentLoseCount = 0;
        currentWinCount++;

        UpdateFace(winFaceMaps, currentWinCount);
    }

    private void HandleLose()
    {
        currentWinCount = 0;
        currentLoseCount++;

        UpdateFace(loseFaceMaps, currentLoseCount);
    }

    private void UpdateFace(CountFaceMap[] maps, int count)
    {
        var map = maps.FirstOrDefault(m => m.count == count);

        // If no specific map found for this count, fallback to the first one (default)
        if (map.Equals(default(CountFaceMap)))
        {
             if (maps.Length > 0)
             {
                 SetFace(maps[0]);
             }
        }
        else
        {
            SetFace(map);
        }
    }

    private void SetFace(CountFaceMap map)
    {
        if (faceImage != null) faceImage.sprite = map.face;
        if (eyeImage != null) eyeImage.enabled = map.needEye;
    }
}
