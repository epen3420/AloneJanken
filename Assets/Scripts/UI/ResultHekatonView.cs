using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResultHekatonView : MonoBehaviour
{
    [System.Serializable]
    private struct SceneHekatonMap
    {
        public string SceneName;
        public Sprite sprite;
    }

    [SerializeField]
    private SceneHekatonMap[] sceneHekatonMaps;
    [SerializeField]
    private Image image;


    private void Awake()
    {
        image.sprite = sceneHekatonMaps.FirstOrDefault(map => map.SceneName == SceneController.CurrentSceneName).sprite;
    }
}
