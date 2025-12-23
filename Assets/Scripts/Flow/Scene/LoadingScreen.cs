using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class LoadingScreen : MonoBehaviour
{
    protected static LoadingScreen Instance;

    [SerializeField]
    private SliderController sliderController;

    private CanvasGroup canvasGroup;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;

        SceneController.OnStartLoading += Execute;
    }

    private void Execute()
    {
        canvasGroup.blocksRaycasts = true;
        Fade(1f).Forget();

        SceneController.OnLoadingScene += SetView;
        SceneController.OnLoadedScene += Disable;
    }

    private void SetView(float progress)
    {
        sliderController.UpdateSliderWithEasing(progress, 0.2f).Forget();
    }

    private void Disable(string _ = "")
    {
        SceneController.OnLoadingScene -= SetView;
        SceneController.OnLoadedScene -= Disable;

        Fade(0f).ContinueWith(() =>
        {
            canvasGroup.blocksRaycasts = false;
            sliderController.UpdateSlider(0);
        }).Forget();
    }

    private void OnDestroy()
    {
        SceneController.OnStartLoading -= Execute;
        SceneController.OnLoadingScene -= SetView;
        SceneController.OnLoadedScene -= Disable;
    }

    private async UniTask Fade(float to)
    {
        await DOTween
            .To(
                () => canvasGroup.alpha,
                x => canvasGroup.alpha = x,
                to,
                SceneController.FADE_DURATION)
            .ToUniTask(cancellationToken: destroyCancellationToken);
    }
}
