using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public void LoadScavengingScene(GameObject levelPrefab)
    {
        StartCoroutine(LoadScavengingSceneCoroutine(levelPrefab));
    }

    public IEnumerator LoadScavengingSceneCoroutine(GameObject levelPrefab)
    {
        // Set scene variable to unload it at the end of the coroutine. 
        Scene currentScene = SceneManager.GetActiveScene();

        // Load new scene in background. 
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("ScavengingScene", LoadSceneMode.Additive);

        // Wait until the last operation fully loads. 
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        // Set newly loaded scene as active. 
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("ScavengingScene"));

        // Instantiate fade in canvas. 
        GameObject levelInstance = Instantiate(levelPrefab);
        levelInstance.transform.position = Vector3.zero;

        // Unload previous scene here so it doesn't block fade in canvas and so you don't see both scenes. 
        SceneManager.UnloadSceneAsync(currentScene);
    }
}