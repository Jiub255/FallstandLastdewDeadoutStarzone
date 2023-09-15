using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField]
    private SaveSlotsMenu _saveSlotsMenu;

    [Header("Menu Buttons")]
    [SerializeField]
    private Button _newGameButton;
    [SerializeField]
    private Button _continueButton;
    [SerializeField]
    private Button _loadGameButton;

    private DataPersistenceManager DataPersistenceManager { get; set; } 

    protected override void OnEnable()
    {
        base.OnEnable();

        GameManager.OnDataPersistenceManagerCreated += InitializeMenu;
    }

    protected void OnDisable()
    {
        GameManager.OnDataPersistenceManagerCreated -= InitializeMenu;
    }

    private void InitializeMenu(DataPersistenceManager dataPersistenceManager)
    {
        DataPersistenceManager = dataPersistenceManager;

        // This method needs DataPersistenceManager. 
        DisableButtonsDependingOnData();

        Debug.Log($"InitializeMenu called from MainMenu. DataPersistenceManager == null: {DataPersistenceManager == null}");
    }

    private void DisableButtonsDependingOnData()
    {
        if (!DataPersistenceManager.HasGameData())
        {
            Debug.Log("No Saved Game Data");

            _continueButton.interactable = false;
            _loadGameButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        _saveSlotsMenu.ActivateMenu(false);
        DeactivateMenu();

        /*        DisableAllButtons();

                // Initialize Player/SO's
                // Do I need to initialize the scene after it's loaded too?
                // Can't do it from here since ChangeScene unloads this scene which destroys this script
                DataPersistenceManager.instance.NewGame(); // ??

                // Set HUD to active
                MasterSingleton.Instance.Canvas.transform.GetChild(5).gameObject.SetActive(true);

                // Load FirstScene
                MasterSingleton.Instance.SceneTransitionManager.ChangeScene("FirstScene", Vector2.zero);
        */
        #region Notes on loading
        // Load FirstScene async

        // Use SceneChangeManager singleton? Or SceneTransition script?

        // Wait until loaded

        // Set new scene as active
        // Is this necessary? Not instantiating anything, at least not yet

        // Initialize Player/Scene for new game

        // Unload this scene
        #endregion
    }

    public void OnLoadGameClicked()
    {
        _saveSlotsMenu.ActivateMenu(true);
        DeactivateMenu();
    }

    public void OnContinueClicked()
    {
        DisableAllButtons();

        // Don't think I want to do persistence between scenes this way
        // Using SO's instead
        // Save the game anytime before loading a new scene
        //DataPersistenceManager.instance.SaveGame();

        // Initialize Player/SO's
        // Do I need to initialize the scene after it's loaded too?
        // Can't do it from here since ChangeScene unloads this scene which destroys this script
        DataPersistenceManager.LoadGame(); // ??

        // TODO - Do this using the correct references. 
        // Set HUD to active
//        MasterSingleton.Instance.Canvas.transform.GetChild(5).gameObject.SetActive(true);

        // Load FirstScene
//        MasterSingleton.Instance.SceneTransitionManager.ChangeScene("FirstScene", Vector2.zero);
    }

    private void DisableAllButtons()
    {
        _newGameButton.interactable = false;
        _continueButton.interactable = false;
        _loadGameButton.interactable = false;
    }

    public void ActivateMenu()
    {
        gameObject.SetActive(true);
        DisableButtonsDependingOnData();
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }
}