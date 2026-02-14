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
        if (maps == null || maps.Length == 0) return;

        // Find the map with the highest count that is less than or equal to the current count
        // maps should be sorted or we sort them effectively by ordering results
        var validMaps = maps.Where(m => m.count <= count);

        if (validMaps.Any())
        {
            var map = validMaps.OrderByDescending(m => m.count).First();
            SetFace(map);
        }
        else
        {
            // Fallback: If no map matches (e.g. count is lower than the smallest threshold),
            // use the one with the smallest count (usually index 0 if sorted, but we sort to be safe)
            // This ensures we switch to 'some' face mode (Win/Lose) even if count is low.
            var map = maps.OrderBy(m => m.count).First();
            SetFace(map);
        }
    }

    private void SetFace(CountFaceMap map)
    {
        if (faceImage != null) faceImage.sprite = map.face;
        if (eyeImage != null) eyeImage.enabled = map.needEye;
    }
}
