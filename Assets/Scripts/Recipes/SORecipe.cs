using System.Collections.Generic;
using UnityEngine;

// Should this be an interface instead? ICraftable?
// No, because the whole point of these SOs is making them in the inspector. 
// SOBuildingRecipe and SOItem inherit this. 
public abstract class SORecipe : ScriptableObject
{
	public string Description = "Enter Description";
	public Sprite Icon;

//	[Header("Total of all PCs' stats must be at this level or higher")]
//	public List<StatRequirement> CombinedStatRequirements;
	[Tooltip("For each stat requirement, must have at least one PC that meets or exceeds it. ")]
	public List<StatRequirement> MinSinglePCStatRequirements;

	// TODO - Could have other requirements for recipes, like having a certain item in your inventory,
	// or having found a blueprint, or having a certain building (like a workbench or whatever). 
	public List<SOBuildingRecipe> RequiredBuildings;
	public List<SOItem> RequiredItems;

	public List<RecipeCost> RecipeCosts;

	// Called by button on Recipe Slot. 
	public abstract void OnClickRecipe();
}