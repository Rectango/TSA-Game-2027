using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneKey : MonoBehaviour
{
    public string sceneToLoad;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
