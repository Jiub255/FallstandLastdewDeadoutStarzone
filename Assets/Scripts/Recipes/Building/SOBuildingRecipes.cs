using System.Collections.Generic;
/*using UnityEditor;*/
using UnityEngine;

// Use this class to collect a list of all created SOBuildingRecipe assets. 
// Then another class can filter that list down to the ones you are able to build (StatRequirement-wise). 
/// <summary>
/// Fill this with all Buildable buildings in the game before building game. 
/// </summary>
[CreateAssetMenu(menuName = "Recipes/Building/SOBuildingRecipes", fileName = "New Building Recipes SO")]
public class SOBuildingRecipes : ScriptableObject/* : SORecipeList*/
{
    private List<SOBuilding> _allBuildingRecipes; 

    public List<SOBuilding> AllBuildingRecipes { get { return _allBuildingRecipes; } }


/*    private List<SORecipe> _buildingRecipes = new();

    /// <summary>
    /// JUST FOR TESTING. USES UNITYEDITOR AND WONT WORK FOR BUILD. <br/>
    /// FILL SO RECIPE LISTS MANUALLY OR SOMEHOW BEFORE BUILD AND GET RID OF THIS STUFF. <br/>
    /// Populates SOInventoryData.PossibleBuildingRecipes by getting all SOBuildingRecipes from the assets folder. 
    /// </summary>
    public List<SORecipe> AllBuildingRecipes { get { return _buildingRecipes; } }

    /// <summary>
    /// JUST FOR TESTING. USES UNITYEDITOR AND WONT WORK FOR BUILD. <br/>
    /// FILL SO RECIPE LISTS MANUALLY OR SOMEHOW BEFORE BUILD AND GET RID OF THIS STUFF. <br/>
    /// Populates SOInventoryData.PossibleBuildingRecipes by getting all SOBuildingRecipes from the assets folder. 
    /// </summary>
    public override void GetAllRecipes()
    {
        _buildingRecipes.Clear();
        
        // TODO - Using UnityEditor here, so need to find another way, or do this before building the game. 
        // Maybe run this method manually right before building? 
        string[] assetNames = AssetDatabase.FindAssets(
            "t:SOBuildingRecipe", 
            new[] { "Assets/SOs/Recipes/Building" });

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var character = AssetDatabase.LoadAssetAtPath<SOBuilding>(SOpath);
            _buildingRecipes.Add(character);
        }

        Debug.Log($"Buildings list length: {_buildingRecipes.Count}");
    }*/
}