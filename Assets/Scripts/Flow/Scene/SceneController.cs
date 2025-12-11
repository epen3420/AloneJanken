using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using TimeSpan = System.TimeSpan;

public static class SceneController
{
    private static bool isInit = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if (isInit) return;

        if (string.IsNullOrEmpty(CurrentSceneName))
        {
            CurrentSceneName = SceneManager.GetActiveScene().name;
        }

        isInit = true;
    }


    public static event System.Action OnStartLoading;
    public static event System.Action<float> OnLoadingScene;
    public static event System.Action OnLoadedScene;
    public static string CurrentSceneName { get; private set; }
    public static string PreviousSceneName { get; private set; }

    private static bool isLoading = false;

    private const float MIN_LOAD_DURATION = 2.0f;


    public static void LoadScene(string name)
    {
        InternalLoadScene(name, true).Forget();
    }

    public static void LoadSceneImmediately(string name)
    {
        InternalLoadScene(name, false).Forget();
    }

    private static async UniTask InternalLoadScene(string name, bool isAsync = false)
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

        Init();

        PreviousSceneName = CurrentSceneName;
        CurrentSceneName = name;

        OnStartLoading?.Invoke();

        if (isAsync)
        {
            await LoadAsync(name);
        }
        else
        {
            SceneManager.LoadScene(name);
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
