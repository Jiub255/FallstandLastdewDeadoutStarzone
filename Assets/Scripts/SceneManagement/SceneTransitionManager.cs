using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField]
    private SOListSOPC _pcSOListSO;

    public void LoadHomeScene()
    {
        StartCoroutine(LoadHomeSceneCoroutine());
    }

    public void LoadScavengingScene(GameObject levelPrefab)
    {
        foreach (SOPC soPC in _pcSOListSO.SOPCs)
        {
            soPC.PCInstance = null;
        }
        S.I.IM.ChangeGameState(GameStates.Combat);
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

        // Instantiate scavenging level. 
        Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);

        // Unload previous scene here so it doesn't block fade in canvas and so you don't see both scenes. 
        SceneManager.UnloadSceneAsync(currentScene);
    }

    public IEnumerator LoadHomeSceneCoroutine()
    {
        // Set scene variable to unload it at the end of the coroutine. 
        Scene currentScene = SceneManager.GetActiveScene();

        // Load new scene in background. 
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("HomeScene", LoadSceneMode.Additive);

        // Wait until the last operation fully loads. 
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        // Set newly loaded scene as active. 
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("HomeScene"));

        // Unload previous scene here so it doesn't block fade in canvas and so you don't see both scenes. 
        SceneManager.UnloadSceneAsync(currentScene);
    }
}