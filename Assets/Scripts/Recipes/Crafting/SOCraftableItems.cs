using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// Use this class to collect a list of all created SOItem assets with a non-empty RecipeCost list, ie. craftable items. 
// Then another class can filter that list down to the ones you are able to build (StatRequirement-wise). 
[CreateAssetMenu(menuName = "Recipes/Crafting/SOCraftableItems", fileName = "New Craftable Items SO")]
public class SOCraftableItems : RecipeList
{
    public override List<SORecipe> GetAllRecipes()
    {
        List<SORecipe> itemSOs = new();

        // TODO - Using UnityEditor here, so need to find another way, or do this before building the game. 
        // Maybe run this method manually right before building? 
        string[] assetNames = AssetDatabase.FindAssets(
            "t:SOItem",
            new[] { "Assets/SOs/Recipes/Items" });
        itemSOs.Clear();
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var character = AssetDatabase.LoadAssetAtPath<SOItem>(SOpath);
            itemSOs.Add(character);
        }

        List<SORecipe> Recipes = itemSOs.Where(item => item.RecipeCosts.Count > 0).ToList();

        Debug.Log($"Craftable items list length: {Recipes.Count}");

        return Recipes;
    }
}