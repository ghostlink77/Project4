using UnityEngine;
using UnityEngine.SceneManagement;

public enum ESceneType
{
    Title,
    Lobby,
    InGame
}


public class SceneLoader : SingletonBehaviour<SceneLoader>
{
    public void LoadScene(ESceneType sceneType)
    {
        Debug.Log($"Load {sceneType} scene.");

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneType.ToString());
    }

    public void ReloadScene()
    {
        Debug.Log($"Reload {SceneManager.GetActiveScene().name} scene.");

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public AsyncOperation LoadSceneAsync(ESceneType sceneType)
    {
        Debug.Log($"Load {sceneType} scene async.");

        Time.timeScale = 1f;
        return SceneManager.LoadSceneAsync(sceneType.ToString());
    }
}
