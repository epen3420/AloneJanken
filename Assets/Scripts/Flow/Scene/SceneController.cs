using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using TimeSpan = System.TimeSpan;

public static class SceneController
{
    public static event System.Action OnStartLoading;
    public static event System.Action<float> OnLoadingScene;
    public static event System.Action OnLoadedScene;

    private static bool isLoading = false;

    private const float MIN_LOAD_DURATION = 2.0f;

    public static void LoadScene(string name)
    {
        InternalLoadScene(name).Forget();
    }

    public static async UniTask InternalLoadScene(string name)
    {
        if (isLoading) return;
        isLoading = true;

        OnStartLoading?.Invoke();

        float timer = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        operation.allowSceneActivation = false;

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

        OnLoadedScene?.Invoke();

        isLoading = false;
    }
}
