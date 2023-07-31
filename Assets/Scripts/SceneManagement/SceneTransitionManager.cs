using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static event Func<float, IEnumerator> OnFadeOut;
    public static event Func<float, IEnumerator> OnFadeIn;

    [SerializeField]
    private SOCurrentTeam _pcSOListSO;
    [SerializeField]
    private float _fadeTime = 0.5f;

    public void LoadHomeScene()
    {
        // Set all PCInstance references to null, and they'll get repopulated by SpawnPoint on scene load. 
        foreach (SOPCData pcDataSO in _pcSOListSO.HomeSOPCSList)
        {
            pcDataSO.PCInstance = null;
        }
        // TODO - Use game state machine instead. 
        S.I.GSM.ChangeGameStateTo(S.I.GSM.Home());
        StartCoroutine(LoadSceneCoroutine("HomeScene"));
    }

    /// <summary>
    /// Loads HomeScene and handles fade in/fade out canvases. 
    /// </summary>
    /// <remarks>
    /// Sends OnFadeOut Func event to Fade canvas object in previous scene to fade out, and
    /// sends OnFadeIn Func event to HomeScene after loading. 
    /// </remarks>
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // Pause gameplay/ disable controls. 
//        S.I.GameManager.Pause(true);

        // Fade to black or whatever. 
        // Send event to some fade UI object in whichever scene is open and active? 
        yield return OnFadeOut?.Invoke(_fadeTime);

        // Cache current scene index to unload after setting HomeScene to active. 
        int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the new scene. Do this async? Or does it matter if the frame stalls since the screen is blank by now? 
        yield return LoadScene(sceneName);

        // Initialize new scene(instantiate PCs, enemies, etc.). 
        // Some stuff happens automatically through OnEnable/Awake/Start, like PCInstantiator. 


        // Once HomeScene is loaded (and initialized? Or initialize after?), set it as the active scene. 
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        // Unload 
        yield return UnloadScene(currentSceneBuildIndex);

        // Fade back in from black. 
        // Send event to some fade UI object in whichever scene is open and active? 
        // How to do this? Can this coroutine have a while(other coroutine is running) thing?
        yield return OnFadeIn?.Invoke(_fadeTime);

        // Unpause gameplay/ reenable controls.
//        S.I.GameManager.Pause(false);
    }

    private IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loadSceneAsync.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator UnloadScene(int sceneBuildIndex)
    {
        AsyncOperation unloadSceneAsync = SceneManager.UnloadSceneAsync(sceneBuildIndex);
        while (!unloadSceneAsync.isDone)
        {
            yield return null;
        }
    }

    public void LoadScavengingScene(GameObject levelPrefab)
    {
        // Set all PCInstance references to null, and they'll get repopulated by SpawnPoint on scene load. 
        foreach (SOPCData pcDataSO in _pcSOListSO.HomeSOPCSList)
        {
            pcDataSO.PCInstance = null;
        }
        S.I.GSM.ChangeGameStateTo(S.I.GSM.Combat());
        StartCoroutine(LoadSceneCoroutine("ScavengingScene"));
    }


/*    public void LoadHomeScene()
    {
        StartCoroutine(LoadHomeSceneCoroutine());
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

        // Make pathfinding grid graph. 


        // Unload previous scene. 
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

        // Unload previous scene. 
        SceneManager.UnloadSceneAsync(currentScene);
    }*/
}