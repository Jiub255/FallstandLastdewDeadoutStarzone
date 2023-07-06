using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public abstract class RecipeList : ScriptableObject
{
	public abstract List<SORecipe> Recipes { get; }
	public abstract void PopulateList();
}