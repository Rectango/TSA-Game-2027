using UnityEngine;
// You must include this namespace to manage scenes
using UnityEngine.SceneManagement; 

public class SceneChanger : MonoBehaviour
{
    // Swap by typing the exact string name of the scene
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Swap by using the build index number (faster performance)
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
