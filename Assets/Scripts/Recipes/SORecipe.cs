using System.Collections.Generic;
using UnityEngine;

// SOBuildingRecipe and SOItem inherit this. 
public abstract class SORecipe : ScriptableObject
{
	[SerializeField, Header("-------------- Item Data ---------------")]
	private string _description = "Enter Description";
	[SerializeField]
	private Sprite _icon;
	[SerializeField, Header("------------- Recipe Data --------------"), Tooltip("For each stat requirement, must have at least one PC that meets or exceeds it. ")]
	private List<StatValue> _minSinglePCStatRequirements;
	[SerializeField, Tooltip("Must have each of these buildings built to be able to craft this recipe. ")]
	private List<SOBuildingItem> _requiredBuildings;
	[SerializeField, Tooltip("Must have each of these items in inventory to be able to craft this recipe. ")]
	private List<SOItem> _requiredItems;
	[SerializeField]
	private List<RecipeCost> _recipeCosts;

	public string Description { get { return _description; } }
	public Sprite Icon { get { return _icon; } }
	public List<StatValue> MinSinglePCStatRequirements { get { return _minSinglePCStatRequirements;} }
	public List<SOBuildingItem> RequiredBuildings { get { return _requiredBuildings; } }
	public List<SOItem> RequiredItems { get { return _requiredItems; } }
	public List<RecipeCost> RecipeCosts { get { return _recipeCosts; } }

	// Called by button on Recipe Slot. 
	public abstract void OnClickRecipe();
}