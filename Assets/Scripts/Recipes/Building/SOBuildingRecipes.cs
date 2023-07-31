using System.Collections.Generic;
using System.Linq;
/*using UnityEditor;*/
using UnityEngine;

// Use this class to collect a list of all created SOBuildingRecipe assets. 
// Then another class can filter that list down to the ones you are able to build (StatRequirement-wise). 
/// <summary>
/// Fill this with all Buildable buildings in the game before building game. 
/// </summary>
[CreateAssetMenu(menuName = "Data/SOBuildingRecipes", fileName = "New Building Recipes SO")]
public class SOBuildingRecipes : ScriptableObject/* : SORecipeList*/
{
    [SerializeField, Header("Put all building that should be buildable in game here\n(Buildings with empty RecipeCost lists get filtered out on game start)")]
    private List<SOBuilding> _buildingsWithRecipeCosts;

    /// <summary>
    /// Put all building that should be buildable in game here (Buildings with empty RecipeCost lists get filtered out on game start). 
    /// </summary>
    public List<SOBuilding> BuildingsWithRecipeCosts { get { return _buildingsWithRecipeCosts; } }

    /// <summary>
    /// Do this once on load to make sure no SOItems with empty recipe cost lists got in. 
    /// TODO - Do this in editor right before build instead? Then won't have to do it each time the game loads. 
    /// </summary>
    public void FilterOutNoRecipeItems()
    {
        int prefilteredListCount = _buildingsWithRecipeCosts.Count;

        _buildingsWithRecipeCosts = _buildingsWithRecipeCosts.Where(item => item.RecipeCosts.Count > 0).ToList();

        int postfilteredListCount = _buildingsWithRecipeCosts.Count;

        if (prefilteredListCount != postfilteredListCount)
        {
            Debug.LogWarning($"{prefilteredListCount - postfilteredListCount} SOBuildings found on list with no recipe costs. ");
        }
    }

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