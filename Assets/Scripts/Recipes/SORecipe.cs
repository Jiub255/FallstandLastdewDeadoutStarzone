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
	private List<SOBuilding> _requiredBuildings;
	[SerializeField, Tooltip("Must have each of these tools in inventory to be able to craft this recipe. ")]
	private List<SOTool> _requiredTools;
	[SerializeField]
	private List<RecipeCost> _recipeCosts;

	public string Description { get { return _description; } }
	public Sprite Icon { get { return _icon; } }
	public List<StatValue> MinSinglePCStatRequirements { get { return _minSinglePCStatRequirements;} }
	public List<SOBuilding> RequiredBuildings { get { return _requiredBuildings; } }
	/// <summary>
	/// Have a separate SOTool:SOItem class? They won't be usable from menu but will satisfy these types of requirements. <br/>
	/// Have things like knife, axe, welder, etc. Can't cut fabric without a knife, can't weld without a welder etc. 
	/// </summary>
	public List<SOTool> RequiredTools { get { return _requiredTools; } }
	public List<RecipeCost> RecipeCosts { get { return _recipeCosts; } }

	// Called by button on Recipe Slot. 
	public abstract void OnClickRecipe();
}