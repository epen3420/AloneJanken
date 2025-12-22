using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using TimeSpan = System.TimeSpan;

public static class SceneController
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if (string.IsNullOrEmpty(CurrentSceneName))
        {
            CurrentSceneName = SceneManager.GetActiveScene().name;
        }
    }


    public static event System.Action OnStartLoading;
    public static event System.Action<float> OnLoadingScene;
    public static event System.Action OnLoadedScene;
    public static string CurrentSceneName { get; private set; } = "";
    public static string PreviousSceneName { get; private set; } = "";
    public static float FADE_DURATION = 0.2f;

    private static bool isLoading = false;

    private const float MIN_LOAD_DURATION = 2.0f;


    public static void LoadScene(string name, LoadMethodType method = LoadMethodType.Async)
    {
        InternalLoadScene(name, method).Forget();
    }

    private static async UniTask InternalLoadScene(string name, LoadMethodType method)
    {
        if (isLoading) return;
        isLoading = true;

        if (name == "Current")
        {
            name = CurrentSceneName;
        }
        else if (name == "Previous")
        {
            name = PreviousSceneName;
        }

        PreviousSceneName = CurrentSceneName;
        CurrentSceneName = name;

        OnStartLoading?.Invoke();

        switch (method)
        {
            case LoadMethodType.Async:
                await LoadAsync(name);
                break;
            case LoadMethodType.Immediate:
                await UniTask.Delay(TimeSpan.FromSeconds(FADE_DURATION));
                SceneManager.LoadScene(name);
                await UniTask.Delay(TimeSpan.FromSeconds(FADE_DURATION));
                break;
        }

        OnLoadedScene?.Invoke();

        isLoading = false;
    }

    private static async UniTask LoadAsync(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        operation.allowSceneActivation = false;

        float timer = 0f;
        while (!operation.isDone)
        {
            timer += Time.deltaTime;

            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            float fakeProgress = Mathf.Clamp01(timer / MIN_LOAD_DURATION);

            float displayProgress = Mathf.Min(realProgress, fakeProgress);

            OnLoadingScene?.Invoke(displayProgress);

            if (realProgress >= 1f && fakeProgress >= 1f)
            {
                OnLoadingScene?.Invoke(1.0f);
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                operation.allowSceneActivation = true;
            }

            await UniTask.DelayFrame(1);
        }
    }
}
