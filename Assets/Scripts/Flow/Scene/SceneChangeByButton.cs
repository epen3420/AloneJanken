using UnityEngine;
using UnityEngine.UI;

public class SceneChangeByButton : MonoBehaviour
{
    [System.Serializable]
    private struct SceneButtonMap
    {
        public string sceneName;
        public Button button;
    }

    [SerializeField]
    private SceneButtonMap[] sceneButtonMaps;


    private void OnEnable()
    {
        foreach (var map in sceneButtonMaps)
        {
            map.button.onClick.AddListener(() => SceneController.LoadScene(map.sceneName));
        }
    }

    private void OnDisable()
    {
        foreach (var map in sceneButtonMaps)
        {
            map.button.onClick.RemoveAllListeners();
        }
    }
}
