using System.Collections.Generic;
/*using System.Linq;
using UnityEditor;*/
using UnityEngine;

// Use this class to collect a list of all created SOItem assets with a non-empty RecipeCost list, ie. craftable items. 
// Then another class can filter that list down to the ones you are able to build (StatRequirement-wise). 
/// <summary>
/// Fill this with all SOItems with non empty RecipeCost lists before building game. 
/// </summary>
[CreateAssetMenu(menuName = "Recipes/Crafting/SOCraftableItems", fileName = "New Craftable Items SO")]
public class SOCraftableItems : ScriptableObject/* : SORecipeList*/
{
    private List<SOItem> _itemsWithRecipeCosts = new();

    public List<SOItem> ItemsWithRecipeCosts { get { return _itemsWithRecipeCosts; } }


/*    /// <summary>
    /// JUST FOR TESTING. USES UNITYEDITOR AND WONT WORK FOR BUILD. <br/>
    /// FILL SO RECIPE LISTS MANUALLY OR SOMEHOW BEFORE BUILD AND GET RID OF THIS STUFF. <br/>
    /// Populates SOInventoryData.PossibleCraftingRecipes by getting all SOItems from the assets folder, then filtering out
    /// the ones with no recipe costs (Items with no recipe costs are considered uncraftable). 
    /// </summary>*/

/*    /// <summary>
    /// JUST FOR TESTING. USES UNITYEDITOR AND WONT WORK FOR BUILD. <br/>
    /// FILL SO RECIPE LISTS MANUALLY OR SOMEHOW BEFORE BUILD AND GET RID OF THIS STUFF. <br/>
    /// Populates SOInventoryData.PossibleCraftingRecipes by getting all SOItems from the assets folder, then filtering out
    /// the ones with no recipe costs (Items with no recipe costs are considered uncraftable). 
    /// </summary>
    public override void GetAllRecipes()
    {
        _itemsWithRecipeCosts.Clear();

        List<SORecipe> allItems = new();

        // TODO - Using UnityEditor here, so need to find another way, or do this before building the game. 
        // Maybe run this method manually right before building? 
        string[] assetNames = AssetDatabase.FindAssets(
            "t:SOItem",
            new[] { "Assets/SOs/Recipes/Items" });

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var character = AssetDatabase.LoadAssetAtPath<SOItem>(SOpath);
            allItems.Add(character);
        }

        _itemsWithRecipeCosts = allItems.Where(item => item.RecipeCosts.Count > 0).ToList();

        Debug.Log($"Items with recipe costs list length: {_itemsWithRecipeCosts.Count}");
    }*/
}