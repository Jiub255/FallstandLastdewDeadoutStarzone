using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField]
    private SOListSOPC _pcSOListSO;
    [SerializeField]
    private float _fadeTime = 0.5f;

    public void LoadHomeScene2()
    {
        StartCoroutine(LoadHomeScene2Coroutine());
    }

    /// <summary>
    /// This method beep boops.
    /// </summary>
    /// <remarks>
    /// This method exists. asldkf asdf asdlk asdk asd vja.s dvlsad.v asdv sadvl sadvn esiandlvi . salvd s.a d
    /// asdlvk dls.as dvkas.dv kjdsamndv. asdfasdfasdf asdf asdfasdfas df af asdf asdf afas fa dfas fasd fasd asd ad asd adas das das d
    /// This method beep boops. asldkf asdf asdlk asdk asd vja.s dvlsad.v asdv sadvl sadvn esiandlvi . salvd s.a d
    /// asdlvk dls.as dvkas.dv kjdsamndv. asdfasdfasdf asdf asdfasdfas df af asdf asdf afas fa dfas fasd fasd asd ad asd adas das das d 
    /// </remarks>
    /// <returns>A boop. </returns>
    public IEnumerator LoadHomeScene2Coroutine()
    {
        // Pause gameplay/ disable controls. 
        S.I.GameManager.Pause(true);

        // Fade to black or whatever. 
        // Send event to some fade UI object in whichever scene is open and active? 
        // How to do this? Can this coroutine have a while(other coroutine is running) thing?
        yield return new WaitForSeconds(_fadeTime);

        // Load the new scene. 
        SceneManager.LoadScene("HomeScene", LoadSceneMode.Additive);

        // Initialize new scene(instantiate PCs, enemies, etc.). 
        // Some stuff happens automatically through OnEnable/Awake/Start, like PCInstantiator. 


        // Fade back in from black. 
        // Send event to some fade UI object in whichever scene is open and active? 
        // How to do this? Can this coroutine have a while(other coroutine is running) thing?
        yield return new WaitForSeconds(_fadeTime);

        // Unpause gameplay/ reenable controls.
        S.I.GameManager.Pause(false);

    }

    public void LoadHomeScene()
    {
        StartCoroutine(LoadHomeSceneCoroutine());
    }

    public void LoadScavengingScene(GameObject levelPrefab)
    {
        foreach (SOPC soPC in _pcSOListSO.HomeSOPCSList)
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
    }
}