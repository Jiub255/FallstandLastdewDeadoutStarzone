using System.Collections.Generic;
using UnityEngine;

public abstract class SORecipeList : ScriptableObject
{
    //	public List<SORecipe> Recipes;
    public abstract /*List<SORecipe>*/void GetAllRecipes();
}