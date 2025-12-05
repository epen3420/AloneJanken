using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void GoToNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
