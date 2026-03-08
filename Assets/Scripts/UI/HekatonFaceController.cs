using System;
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
        public Sprite FaceSprite;
        public bool NeedEye;
    }

    [Header("UI References")]
    [SerializeField] private Image faceImage;
    [SerializeField] private Image eyeImage;

    [Header("Settings")]
    [SerializeField] private Face defaultFace;
    [SerializeField] private Face winFace;
    [SerializeField] private Face loseFace;
    [SerializeField] private float judgeFaceDuration = 0.5f;
    [SerializeField] private CountFaceMap[] winFaceMaps;
    [SerializeField] private CountFaceMap[] loseFaceMaps;

    [Header("Events")]
    [SerializeField] private BoolEventChannelSO endJankenEvent;

    private int currentLoseCount = 0;
    private int currentWinCount = 0;


    private void Start()
    {
        SetFace(defaultFace);
    }

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

    private async void HandleWin()
    {
        currentLoseCount = 0;
        currentWinCount++;

        SetFace(winFace);

        await UniTask.Delay(TimeSpan.FromSeconds(judgeFaceDuration));

        UpdateFace(winFaceMaps, currentWinCount);
    }

    private async void HandleLose()
    {
        currentWinCount = 0;
        currentLoseCount++;
        SetFace(loseFace);

        await UniTask.Delay(TimeSpan.FromSeconds(judgeFaceDuration));

        UpdateFace(loseFaceMaps, currentLoseCount);
    }

    private void UpdateFace(CountFaceMap[] maps, int count)
    {
        if (maps == null || maps.Length == 0) return;

        foreach (var map in maps)
        {
            if (map.count == count)
            {
                SetFace(map.face);
                break;
            }
        }
    }

    private void SetFace(Face face)
    {
        if (face.FaceSprite == null)
            face = defaultFace;

        if (faceImage != null) faceImage.sprite = face.FaceSprite;
        if (eyeImage != null) eyeImage.enabled = face.NeedEye;
    }
}
