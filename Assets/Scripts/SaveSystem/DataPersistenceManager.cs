using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// TODO - Make this a plain C# class and pass in all the SaveableSOs in the constructor from GameManager? <br/>
/// Or just put them in a serialized list here? Try the first way, not sure exactly how yet. Send references through events as they get constructed? <br/>
/// Or just send the GameDataSO, which holds all the others. 
/// </summary>
public class DataPersistenceManager
{
    public event System.Action<GameSaveData> OnSave;
    public event System.Action<GameSaveData> OnLoad;

    private SOSaveSystemData SaveSystemDataSO { get; }

    private GameSaveData GameData { get; set; }

    private GameManager GameManager { get; }

    private FileDataHandler FileDataHandler { get; }

    private string SelectedProfileID { get; set; } = "";

    private Coroutine AutoSaveCoroutine { get; set; }

    public static DataPersistenceManager instance { get; private set; }

    public DataPersistenceManager(GameManager gameManager, SOSaveSystemData saveSystemDataSO)
    {
        GameManager = gameManager;
        SaveSystemDataSO = saveSystemDataSO;

        FileDataHandler = new FileDataHandler(Application.persistentDataPath, SaveSystemDataSO.FileName);
        InitializeSelectedProfileID();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        // TODO - How to actually call these from menu? 
 //       UIStartMenu.OnNew += NewGame;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Start up the autosave coroutine
        if (AutoSaveCoroutine != null)
        {
            GameManager.StopCoroutine(AutoSaveCoroutine);
        }
        AutoSaveCoroutine = GameManager.StartCoroutine(AutoSave());
    }

    public void DeleteProfileData(string profileID)
    {
        // Delete the data for this profile ID
        FileDataHandler.Delete(profileID);
        // Initialize the selected profile ID
        InitializeSelectedProfileID();
        // Reload the game so that our data matches the newly selected profile ID
        LoadGame();
    }

    private void InitializeSelectedProfileID()
    {
        SelectedProfileID = FileDataHandler.GetMostRecentlyUpdatedProfileID();
    }

    public GameSaveData NewGame()
    {
        GameData = new GameSaveData();

        // Push the loaded data to all other scripts that need it.
//        GameManager.LoadData(GameData);
        OnLoad?.Invoke(GameData);

        return GameData;
    }

    // Have this return loaded SaveData so SaveSlotsMenu and MainMenu can load the 
    // current saved scene
    public GameSaveData LoadGame()
    {
        // Load any saved data from a file using the data handler.
        GameData = FileDataHandler.Load(SelectedProfileID);

        // If no data can be loaded, warn player.
        if (GameData == null)
        {
            Debug.Log("No data was found. Start a new game.");
            return null;
        }

        // Push the loaded data to all other scripts that need it.
//        GameManager.LoadData(GameData);
        OnLoad?.Invoke(GameData);

        return GameData;
    }

    public void SaveGame()
    {
        // If no data can be saved, warn player.
        if (GameData == null)
        {
            Debug.Log("No data was found. Start a new game.");
            return;
        }

        // Pass the data to other scripts so they can update it.
        // TODO - Just run the SaveData method on GameManager and have it chain through its children saving all their data? Same for loading. 
        // Won't have to find all ISaveables then either, not sure how to do it without them inheriting Unity.Object. 
//        GameManager.SaveData(GameData);
        OnSave?.Invoke(GameData);

        // Timestamp the data so we know when it was last saved
        GameData.LastUpdated = System.DateTime.Now.ToBinary();

        // Save that data to a file using the data handler.
        FileDataHandler.Save(GameData, SelectedProfileID);
    }

    // Maybe implement an "on quit" save later (in a separate slot). 
/*    private void OnApplicationQuit()
    {
        SaveGame();
    }*/

    // Have this return loaded SaveData so SaveSlotsMenu and MainMenu can load the current saved scene
    public GameSaveData ChangeSelectedProfileID(string newProfileID)
    {
        // Update the profile to use for saving and loading
        SelectedProfileID = newProfileID;
        // Load the game, which will use that profile, updating our game data accordingly
        return LoadGame();
    }

    public bool HasGameData()
    {
        GameData = FileDataHandler.Load(SelectedProfileID);

        Debug.Log("Save data exists: " + (GameData != null).ToString());

        return (GameData != null);
    }

    public Dictionary<string, GameSaveData> GetAllProfilesGameData()
    {
        return FileDataHandler.LoadAllProfiles();
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(SaveSystemDataSO.AutoSaveTimeSeconds);
            SaveGame();
            Debug.Log("Auto Saved Game");
        }
    }
}