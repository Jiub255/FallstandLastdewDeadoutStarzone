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
    [SerializeField]
    private SOGameData _gameDataSO;

    // Managers
    private PCManager PCManager { get; set; }
    private InventoryManager InventoryManager { get; set; }
    private StatManager StatManager { get; set; }
    private BuildingManager BuildingManager { get; set; }

    // Data
    private SOGameData GameDataSO { get { return _gameDataSO; } set { _gameDataSO = value; } }

    private void OnEnable()
    {
        GameDataSO.InventoryDataSO.CraftableItemsList.FilterOutNoRecipeItems();
        GameDataSO.BuildingDataSO.BuildableBuildingsList.FilterOutNoRecipeItems();

        // Instantiate PCManager first, so it can instantiate all the PC's in the game world. 
        PCManager = new(GameDataSO.TeamDataSO);
        InventoryManager = new(GameDataSO.InventoryDataSO);
        StatManager = new(GameDataSO.TeamDataSO);
        BuildingManager = new(GameDataSO.BuildingDataSO);

        InventoryManager.OnInventoryChanged += GetPossibleRecipes;
        PCStatManager.OnStatsChanged += GetPossibleRecipes;
    }

    private void OnDisable()
    {
        // Run OnDisable in created class instances (to unsubscribe from events).  
        InventoryManager.OnDisable();
        PCManager.OnDisable();
        BuildingManager.OnDisable();

        InventoryManager.OnInventoryChanged -= GetPossibleRecipes;
        PCStatManager.OnStatsChanged -= GetPossibleRecipes;
    }

    /// <summary>
    /// First checks StatManager to get all recipes that you meet the stat requirements for. <br/>
    /// Then checks InventoryManager to get all recipes that you also have enough items for. <br/>
    /// TODO - Check BuildingManager for required building stuff, and inv for required held items stuff. 
    /// </summary>
    /// <returns>List of all recipes that you meet requirements for, and have enough items to craft. </returns>
    public void GetPossibleRecipes()
    {
        // Get all craftable items.
        GameDataSO.InventoryDataSO.PossibleCraftingRecipes.Clear();
        List<SOItem> metStatReqsItems = StatManager.GetMetStatRequirementsRecipes(GameDataSO.InventoryDataSO.CraftableItemsList.ItemsWithRecipeCosts);
        List<SOItem> haveReqItemsAndToolsItems = InventoryManager.GetHaveEnoughItemsRecipes(metStatReqsItems);
        GameDataSO.InventoryDataSO.PossibleCraftingRecipes = BuildingManager.GetHaveRequiredBuildingsRecipes(haveReqItemsAndToolsItems);

        // Get all buildable buildings. 
        GameDataSO.InventoryDataSO.PossibleBuildingRecipes.Clear();
        List<SOBuilding> metStatReqsBuildings = StatManager.GetMetStatRequirementsRecipes(GameDataSO.BuildingDataSO.BuildableBuildingsList.BuildingsWithRecipeCosts);
        List<SOBuilding> haveReqItemsAndToolsBuildings = InventoryManager.GetHaveEnoughItemsRecipes(metStatReqsBuildings);
        GameDataSO.InventoryDataSO.PossibleBuildingRecipes = BuildingManager.GetHaveRequiredBuildingsRecipes(haveReqItemsAndToolsBuildings);
    }

    private void Update()
    {
        PCManager.UpdateStates();
    }

    private void FixedUpdate()
    {
        PCManager.FixedUpdateStates();
        BuildingManager.FixedUpdate();
    }
}