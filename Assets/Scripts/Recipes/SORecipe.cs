using System.Collections.Generic;
using UnityEngine;

// SOBuildingRecipe and SOItem inherit this. 
public abstract class SORecipe : ScriptableObject
{
	public string Description = "Enter Description";
	public Sprite Icon;

	[Header("Total of all PCs' stats must be at this level or higher")]
	public List<StatRequirement> StatRequirements;
	[Header("Must have at least one PC with this stat at this level or higher")]
	public List<StatRequirement> MinSinglePCStatRequirements; 

	public List<RecipeCost> RecipeCosts;

	// Called by button on Recipe Slot. 
	public abstract void OnClickRecipe();
}