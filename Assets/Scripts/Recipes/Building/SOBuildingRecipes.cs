using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Use this class to collect a list of all created SOBuildingRecipe assets. 
// Then another class can filter that list down to the ones you are able to build (StatRequirement-wise). 
[CreateAssetMenu(menuName = "Recipes/Building/SOBuildingRecipes", fileName = "New Building Recipes SO")]
public class SOBuildingRecipes : RecipeList
{
    public override List<SORecipe> GetAllRecipes()
    {
        List<SORecipe> Recipes = new();

        // TODO - Using UnityEditor here, so need to find another way, or do this before building the game. 
        // Maybe run this method manually right before building? 
        string[] assetNames = AssetDatabase.FindAssets(
            "t:SOBuildingRecipe", 
            new[] { "Assets/SOs/Recipes/Building" });
        Recipes.Clear();
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var character = AssetDatabase.LoadAssetAtPath<SOBuildingRecipe>(SOpath);
            Recipes.Add(character);
        }

        Debug.Log($"Buildings list length: {Recipes.Count}");

        return Recipes;
    }
}