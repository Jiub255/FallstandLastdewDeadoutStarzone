using System;
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
    [SerializeField]
    private SOCraftableItems _craftingItemsList;
    [SerializeField]
    private SOBuildingRecipes _buildingRecipesList;

    private InventoryManager InventoryManager { get; set; }
    private StatManager StatManager { get; set; }
    private PCManager PCManager { get; set; }
    private SOGameData GameDataSO { get { return _gameDataSO; } set { _gameDataSO = value; } }
    private SOCraftableItems CraftingRecipesList { get { return _craftingItemsList; } set { _craftingItemsList = value; } }
    private SOBuildingRecipes BuildingRecipesList { get { return _buildingRecipesList; } set { _buildingRecipesList = value; } }

    private void OnEnable()
    {
        // TODO - Just for testing, manually run these before building to populate SO recipe lists. 
        CraftingRecipesList.GetAllRecipes();
        BuildingRecipesList.GetAllRecipes();

        InventoryManager = new(GameDataSO.InventoryDataSO);
        StatManager = new(GameDataSO);
        PCManager = new(GameDataSO.CurrentTeamSO);

        SpawnPoint.OnSceneStart += InitializeScene;
    }

    private void OnDisable()
    {
        // Run OnDisable in created class instances. 
        InventoryManager.OnDisable();
        StatManager.OnDisable();
        PCManager.OnDisable();

        SpawnPoint.OnSceneStart -= InitializeScene;
    }

    private void InitializeScene(Vector3 spawnPosition)
    {
        InstantiatePCs(spawnPosition);
    }

    private void InstantiatePCs(Vector3 spawnPosition)
    {
        if (GameDataSO.CurrentTeamSO.HomeSOPCSList.Count > 0)
        {
            for (int i = 0; i < GameDataSO.CurrentTeamSO.HomeSOPCSList.Count; i++)
            {
                // Will UnityEngine.Object.Instantiate work? Or should this be done in GameManager? 
                GameDataSO.CurrentTeamSO.HomeSOPCSList[i].PCInstance = Instantiate(
                    GameDataSO.CurrentTeamSO.HomeSOPCSList[i].PCPrefab,
                    new Vector3(3 * i, 0f, 0f) + spawnPosition,
                    Quaternion.identity);
            }
            
            PCManager.PopulateDictionary();
        }
        else
        {
            Debug.LogWarning("No PCs on HomeSOPCSList in CurrentTeamSO. Can't play the game without PCs. ");
        }
    }

    /// <summary>
    /// First checks StatManager to get all recipes that you meet the stat requirements for. <br/>
    /// Then checks InventoryManager to get all recipes that you also have enough items for. <br/>
    /// TODO - Check BuildingManager for required building stuff, and inv for required held items stuff. 
    /// </summary>
    /// <returns>List of all recipes that you meet requirements for, and have enough items to craft. </returns>
    public List<SORecipe> GetPossibleCraftingRecipes()
    {
        GameDataSO.InventoryDataSO.PossibleCraftingRecipes.Clear();
        List<SORecipe> metStatReqsRecipes = StatManager.GetMetStatRequirementsRecipes(CraftingRecipesList.ItemsWithRecipeCosts);
        GameDataSO.InventoryDataSO.PossibleCraftingRecipes = InventoryManager.GetHaveEnoughItemsRecipes(metStatReqsRecipes);
        return GameDataSO.InventoryDataSO.PossibleCraftingRecipes;
    }

    /// <summary>
    /// First checks StatManager to get all recipes that you meet the stat requirements for. <br/>
    /// Then checks InventoryManager to get all recipes that you also have enough items for. <br/>
    /// TODO - Check BuildingManager for required building stuff, and inv for required held items stuff. 
    /// </summary>
    /// <returns>List of all recipes that you meet requirements for, and have enough items to craft. </returns>
    public List<SORecipe> GetPossibleBuildingRecipes()
    {
        GameDataSO.InventoryDataSO.PossibleBuildingRecipes.Clear();
        List<SORecipe> metStatReqsRecipes = StatManager.GetMetStatRequirementsRecipes(BuildingRecipesList.AllBuildingRecipes);
        GameDataSO.InventoryDataSO.PossibleBuildingRecipes = InventoryManager.GetHaveEnoughItemsRecipes(metStatReqsRecipes);
        return GameDataSO.InventoryDataSO.PossibleBuildingRecipes;
    }

    private void Update()
    {
        PCManager.UpdateStates();
    }

    private void FixedUpdate()
    {
        PCManager.FixedUpdateStates();
    }
}