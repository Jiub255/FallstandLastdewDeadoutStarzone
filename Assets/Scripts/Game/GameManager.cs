using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates PCManager, StatsManager, and InventoryManager. <br/>
/// TODO - Eventually have all managers (UI, SceneTransition, Audio, Building, etc.) here, and this be the only monobehaviour? 
/// </summary>
/// <remarks>
/// TODO - Eventually replace the singleton with this? <br/> 
/// OR, make/put this in the home/combat game states? (ie non-menu gameplay) <br/> 
/// No, inventory and stats should be accessible from menus. Just put the PCManager and EnemyManagers in home/combat game states? 
/// </remarks>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Any class that needs InputManager but isn't created by GameManager gets the reference through this. 
    /// </summary>
    public static event System.Action<InputManager> OnInputManagerCreated;
    public static event System.Action<GameStateMachine> OnGameStateMachineCreated;
    /// <summary>
    /// Heard by UIRecipes, calls SetupRecipeSlots. 
    /// </summary>
    public static event System.Action OnRecipeListsCalculated;

    [SerializeField]
    private SOGameData _gameDataSO;

    // Managers
    private PCManager PCManager { get; set; }
    private InventoryManager InventoryManager { get; set; }
    private StatManager StatManager { get; set; }
    private BuildingManager BuildingManager { get; set; }
    private GameStateMachine GameStateMachine { get; set; }
    private InputManager InputManager { get; set; }
    private SceneTransitionController SceneTransitionManager { get; set; }
    private DataPersistenceManager DataPersistenceManager { get; set; }

    // Data
    private SOGameData GameDataSO { get { return _gameDataSO; } set { _gameDataSO = value; } }

    public void SaveData(GameSaveData gameData)
    {
        PCManager.SaveData(gameData);
        InventoryManager.SaveData(gameData);
        BuildingManager.SaveData(gameData);
    }

    public void LoadData(GameSaveData gameData)
    {
        // Do similar to SaveData to get data back into SOs, then apply that data to the game world,
        // for example: instantiate and position all buildings. 
        PCManager.LoadData(gameData);
        InventoryManager.LoadData(gameData);
        BuildingManager.LoadData(gameData);
    }

    // Why not in subscribe to events in OnEnable and new stuff up in Awake? 
    // So that stuff that needs the InputManager reference event can subscribe to it in OnEnable. 
    private void Start()
    {
        InputManager = new();
        OnInputManagerCreated?.Invoke(InputManager);
        // Instantiate PCManager first, so it can instantiate all the PC's in the game world. Before what though? Does it matter? 
        PCManager = new(GameDataSO.TeamDataSO, InputManager, this);
        InventoryManager = new(GameDataSO.InventoryDataSO);
        StatManager = new(GameDataSO.TeamDataSO);
        BuildingManager = new(GameDataSO.BuildingDataSO, InputManager);
        GameStateMachine = new(InputManager, PCManager, BuildingManager);
        OnGameStateMachineCreated?.Invoke(GameStateMachine);
        SceneTransitionManager = new(GameDataSO.TeamDataSO, GameDataSO.SceneTransitionFadeTime, GameStateMachine, this);
        DataPersistenceManager = new(this, GameDataSO.SaveSystemDataSO);

        InventoryManager.OnInventoryChanged += GetPossibleRecipes;
        PCStatManager.OnStatsChanged += GetPossibleRecipes;
        DataPersistenceManager.OnSave += SaveData;
        DataPersistenceManager.OnLoad += LoadData;
        
        GameDataSO.InventoryDataSO.CraftableItemsSO.FilterOutNoRecipeItems();
        GameDataSO.BuildingDataSO.BuildableBuildingsSO.FilterOutNoRecipeItems();
    }

    private void OnDisable()
    {
        // Run OnDisable in created class instances (to unsubscribe from events).  
        InventoryManager.OnDisable();
        PCManager.OnDisable();
        BuildingManager.OnDisable();
        InputManager.OnDisable();
        SceneTransitionManager.OnDisable();
        DataPersistenceManager.OnDisable();
        foreach (SOPCData pcDataSO in GameDataSO.TeamDataSO.HomePCs)
        {
            pcDataSO.PCController.PCStatManager.OnDisable();
        }

        InventoryManager.OnInventoryChanged -= GetPossibleRecipes;
        PCStatManager.OnStatsChanged -= GetPossibleRecipes;
        DataPersistenceManager.OnSave -= SaveData;
        DataPersistenceManager.OnLoad -= LoadData;
    }

    /// <summary>
    /// First checks StatManager to filter out all recipes that you don't meet the stat requirements for. <br/>
    /// Then checks InventoryManager to filter out all recipes that you don't have enough items for, or don't have the required tools for. <br/>
    /// Then finally checks BuildingManager to filter out all recipes that you don't have the required crafting buildings for. <br/>
    /// Does this for both the PossibleCraftingRecipes and PossibleBuildingRecipes lists from SOInventoryData. 
    /// </summary>
    public void GetPossibleRecipes()
    {
        // Get all craftable items.
        List<SOItem> metStatReqsItems = StatManager.GetMetStatRequirementsRecipes(GameDataSO.InventoryDataSO.CraftableItemsSO.ItemsWithRecipeCosts);
        List<SOItem> haveReqItemsAndToolsItems = InventoryManager.GetHaveEnoughItemsRecipes(metStatReqsItems);
        GameDataSO.InventoryDataSO.PossibleCraftingRecipes = BuildingManager.GetHaveRequiredBuildingsRecipes(haveReqItemsAndToolsItems);

        // Get all buildable buildings. 
        List<SOBuilding> metStatReqsBuildings = StatManager.GetMetStatRequirementsRecipes(GameDataSO.BuildingDataSO.BuildableBuildingsSO.BuildingsWithRecipeCosts);
        List<SOBuilding> haveReqItemsAndToolsBuildings = InventoryManager.GetHaveEnoughItemsRecipes(metStatReqsBuildings);
        GameDataSO.InventoryDataSO.PossibleBuildingRecipes = BuildingManager.GetHaveRequiredBuildingsRecipes(haveReqItemsAndToolsBuildings);

        OnRecipeListsCalculated?.Invoke();

        // FOR TESTING
        Debug.Log($"Possible crafting recipes: {GameDataSO.InventoryDataSO.PossibleCraftingRecipes.Count}");
        Debug.Log($"Possible building recipes: {GameDataSO.InventoryDataSO.PossibleBuildingRecipes.Count}");
    }

    /// <summary>
    /// TODO - Call GameStateMachine.UpdateGameStates here? Then have each game state get passed references to the appropriate managers in its constructor
    /// and then call update on them? <br/> 
    /// Like only call PCManager.UpdateStates from non-menu states, only call building manager's from Build state, etc. 
    /// </summary>
    private void Update()
    {
        InputManager.Update();
        GameStateMachine.UpdateActiveState();
    }

    private void FixedUpdate()
    {
        GameStateMachine.FixedUpdateActiveState();
    }
}