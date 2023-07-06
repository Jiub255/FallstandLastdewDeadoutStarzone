using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// Use this class to collect a list of all created SOItem assets with a non-empty RecipeCost list, ie. craftable items. 
// Then another class can filter that list down to the ones you are able to build (StatRequirement-wise). 
[CreateAssetMenu(menuName = "Recipes/Crafting/SOCraftableItems", fileName = "New Craftable Items SO")]
public class SOCraftableItems : RecipeList
{
    private List<SORecipe> _craftableItems = new();

    public override List<SORecipe> Recipes => _craftableItems;

    public override void PopulateList()
    {
        List<SORecipe> items = new();

        // TODO - Using UnityEditor here, so need to find another way, or do this before building the game. 
        string[] assetNames = AssetDatabase.FindAssets(
            "t:SOItem",
            new[] { "Assets/SOs/Recipes/Items" });
        items.Clear();
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var character = AssetDatabase.LoadAssetAtPath<SOItem>(SOpath);
            items.Add(character);
        }

        _craftableItems = items.Where(item => item.RecipeCosts.Count > 0).ToList();
    }
}