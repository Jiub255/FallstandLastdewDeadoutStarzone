using System.Collections.Generic;

//[System.Serializable]
public interface IRecipeList
{
	public abstract List<SORecipe> Recipes { get; }
	public abstract void PopulateList();
}