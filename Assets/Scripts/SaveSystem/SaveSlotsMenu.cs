using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotsMenu : Menu
{
    /// <summary>
    /// Heard by SceneTransitionManager, loads home scene. 
    /// </summary>
    public static event System.Action OnGameSaveDataLoaded;

    [Header("Menu Navigation")]
    [SerializeField]
    private MainMenu _mainMenu;

    [Header("Menu Buttons")]
    [SerializeField]
    private Button _backButton;

    [Header("Confirmation Popup")]
    [SerializeField]
    private ConfirmationPopupMenu _confirmationPopupMenu;

    private SaveSlot[] _saveSlots;

    private bool _loadingSavedGame = false;

    private DataPersistenceManager DataPersistenceManager { get; set; } 

    private void Awake()
    {
        _saveSlots = GetComponentsInChildren<SaveSlot>();

        Debug.Log($"Number of save slots: {_saveSlots.Length}");

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        GameManager.OnDataPersistenceManagerCreated += InitializeMenu; 
    }

    // Using OnDestroy instead of OnDisable because event subscription happens in Awake instead of OnEnable 
    // (since save slots menu is disabled when script loads, but not main/start menu). 
    // TODO - Not sure if this is the best idea, figure it out. 
        private void OnDestroy()
    {
        GameManager.OnDataPersistenceManagerCreated -= InitializeMenu;
    }

    private void InitializeMenu(DataPersistenceManager dataPersistenceManager)
    {
        DataPersistenceManager = dataPersistenceManager;
        
        Debug.Log($"DataPersistenceManager reference assigned");

        gameObject.SetActive(false);
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        // Disable all buttons. 
        DisableMenuButtons();

        // Case: Loading saved game. 
        if (_loadingSavedGame)
        {
            DataPersistenceManager.ChangeSelectedProfileID(saveSlot.GetProfileID());
            SaveGameAndLoadScene();
        }
        // Case: New game, but the save slot has data.
        else if (saveSlot.HasData)
        {
            _confirmationPopupMenu.ActivateMenu(
                "Starting a New Game with this slot will override the currently saved data. " +
                "Are you sure?",
                // Function to execute if "yes" selected.
                () =>
                {
                    Debug.Log("yes new clear clicked");
                    DataPersistenceManager.ChangeSelectedProfileID(
                        saveSlot.GetProfileID());
                    DataPersistenceManager.NewGame();
                    SaveGameAndLoadScene();
                },
                // Function to execute if "cancel" selected.
                () =>
                {
                    Debug.Log("cancel new clear clicked");
                    ActivateMenu(_loadingSavedGame);
                }
            );
        }
        // Case: New game, and the save slot has no data. 
        else
        {
            DataPersistenceManager.ChangeSelectedProfileID(saveSlot.GetProfileID());
            DataPersistenceManager.NewGame();
            SaveGameAndLoadScene();
        }

/*        // Update the selected profile ID to be used for data persistence
        DataPersistenceManager.instance.ChangeSelectedProfileID(saveSlot.GetProfileID());

        if (!isLoadingGame)
        {
            // Create a new game, which will initialize our data to a clean slate
            DataPersistenceManager.instance.NewGame();
        }*/


    }

    private void SaveGameAndLoadScene()
    {
        // Don't think I want to do persistence between scenes this way
        // Using SO's instead
        // Save the game anytime before loading a new scene
        //DataPersistenceManager.instance.SaveGame();

        // Load the scene
        // Could load whichever scene the last save was in, instead of always FirstScene
        // TODO - Which scene to load here? And how to load it? Need to make SceneTransitionManager first. 
        OnGameSaveDataLoaded?.Invoke(); 
 //       MasterSingleton.Instance.SceneTransitionManager.ChangeScene("FirstScene", Vector2.zero);
    }

    public void OnClearClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        _confirmationPopupMenu.ActivateMenu(
            "Are you sure you want to delete this saved data?",
            // Function to execute if we select "yes"
            () =>
            {
                Debug.Log("yes clear clicked");
                DataPersistenceManager.DeleteProfileData(saveSlot.GetProfileID());
                ActivateMenu(_loadingSavedGame);
            },
            // Function to execute if we select "cancel"
            () =>
            {
                // Clearing save data even when clicking cancel
                Debug.Log("cancel clear clicked");
                ActivateMenu(_loadingSavedGame);
            }
        );
    }

    public void OnBackClicked()
    {
        _mainMenu.ActivateMenu();
        DeactivateMenu();
    }

    public void ActivateMenu(bool loadingSavedGame)
    {
        // Set this menu to be active. 
        gameObject.SetActive(true);

        // Set mode. 
        _loadingSavedGame = loadingSavedGame;

        // Load all of the profiles that exist. 
        Dictionary<string, GameSaveData> profilesGameData = DataPersistenceManager.GetAllProfilesGameData();

        // Ensure that the back button is enabled and selected when the menu is activated. 
        _backButton.interactable = true;
        GameObject firstSelected = _backButton.gameObject;

        // Loop through each save slot and set the content appropriately. 
        foreach (SaveSlot saveSlot in _saveSlots)
        {
            GameSaveData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileID(), out profileData);
            saveSlot.SetData(profileData);
            if (profileData == null && loadingSavedGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(_backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }

        // Set the first selected button. 
        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        SetFirstSelected(firstSelectedButton);
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in _saveSlots)
        {
            saveSlot.SetInteractable(false);
        }

        _backButton.interactable = false;
    }
}