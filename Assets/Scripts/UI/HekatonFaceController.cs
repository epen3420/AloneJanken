using TimeSpan = System.TimeSpan;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HekatonFaceController : MonoBehaviour
{
    [System.Serializable]
    private struct CountFaceMap
    {
        public int count;
        public Face face;
    }

    [System.Serializable]
    private struct Face
    {
        public Sprite openMouth;
        public Sprite closeMouth;
    }

    [Header("UI References")]
    [SerializeField] private Image faceImage;

    [Header("Settings")]
    [SerializeField] private Face defaultFace;
    [SerializeField] private Face winFace;
    [SerializeField] private float judgeFaceDuration = 0.5f;
    [SerializeField] private CountFaceMap[] winFaceMaps;
    [SerializeField] private CountFaceMap[] loseFaceMaps;

    [Header("Events")]
    [SerializeField] private BoolEventChannelSO endJankenEvent;
    [SerializeField] private BoolEventChannelSO changeMouthEvent;

    private Face currentFace;
    private int currentLoseCount = 0;
    private int currentWinCount = 0;


    private void Start()
    {
        SetFace(defaultFace, false);
    }

    private void OnEnable()
    {
        if (endJankenEvent != null)
            endJankenEvent.OnRaised += OnJankenEnd;
        if (changeMouthEvent != null)
            changeMouthEvent.OnRaised += ChangeMouthFace;
    }

    private void OnDisable()
    {
        if (endJankenEvent != null)
            endJankenEvent.OnRaised -= OnJankenEnd;
        if (changeMouthEvent != null)
            changeMouthEvent.OnRaised -= ChangeMouthFace;
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

    private async void HandleWin()
    {
        currentWinCount++;

        SetFace(winFace, false);

        await UniTask.Delay(TimeSpan.FromSeconds(judgeFaceDuration));

        UpdateFace(winFaceMaps, currentWinCount);
    }

    private void HandleLose() // awaitがないため async を削除して最適化
    {
        currentWinCount = 0;
        currentLoseCount++;

        UpdateFace(loseFaceMaps, currentLoseCount);
    }

    private void UpdateFace(CountFaceMap[] maps, int count)
    {
        if (maps == null || maps.Length == 0) return;

        var targetMap = maps
            .Where(m => count >= m.count)
            .OrderBy(m => m.count)
            .LastOrDefault();

        Face updateFace = targetMap.face.closeMouth != null ? targetMap.face : defaultFace;

        SetFace(updateFace, false);
    }

    private void ChangeMouthFace(bool isOpen)
    {
        SetFace(currentFace, isOpen);
    }

    private void SetFace(Face face, bool isOpenMouth)
    {
        if (faceImage == null) return;

        currentFace = face.closeMouth != null ? face : defaultFace;

        faceImage.sprite = isOpenMouth ? currentFace.openMouth : currentFace.closeMouth;
    }
}
