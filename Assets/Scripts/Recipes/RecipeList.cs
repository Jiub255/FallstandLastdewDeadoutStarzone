using System.Collections.Generic;
using UnityEngine;

public abstract class RecipeList : ScriptableObject
{
//	public List<SORecipe> Recipes;
	public abstract List<SORecipe> GetAllRecipes();
}