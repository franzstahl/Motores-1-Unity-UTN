using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public void Loadscene(string scene)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void Resume()
    {
        Time.timeScale = 1;
    }
}
