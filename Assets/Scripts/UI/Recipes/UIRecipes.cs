using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Put this on the Crafting and Building canvases. 
/// <summary>
/// TODO - How to decide between Building and Crafting recipes?
/// Might just make two scripts, UIBuilding and UICrafting instead of UIRecipes. 
/// Or this stupid little BuildOrCraft enum might work just fine. 
/// </summary>
public class UIRecipes : MonoBehaviour
{
	private enum BuildOrCraft
    {
		Build,
		Craft
    }

	[SerializeField]
	private BuildOrCraft _buildingOrCraftingMenu;
	[SerializeField]
	private GameObject _recipeSlotPrefab;
	[SerializeField]
	private Transform _slotsParent;

	/// <summary>
	/// Just using to get GameData for now, probably going to add it to the GameManager eventually and have it pass that down in the constructor. 
	/// </summary>
	[SerializeField]
	private SOGameData _gameDataSO;

	private BuildOrCraft BuildingOrCraftingMenu { get { return _buildingOrCraftingMenu; } }
	private GameObject RecipeSlotPrefab { get { return _recipeSlotPrefab; } }
	private Transform SlotsParent { get { return _slotsParent; } }
	private SOGameData GameDataSO { get { return _gameDataSO; } }

	public virtual void OnEnable()
	{
		// Call this whenever stats change. 
		SetupRecipeSlots();

		GameManager.OnRecipeListsCalculated += SetupRecipeSlots;
	}

    private void OnDisable()
    {
		GameManager.OnRecipeListsCalculated -= SetupRecipeSlots;
	}

    // TODO - Gray out the buildings/items that you don't have enough materials to build. 
    // Will have to call this method every time you use some materials to build something. 
    public void SetupRecipeSlots()
	{
//		Debug.Log("SetupRecipeSlots called. ");
		ClearSlots();

		// TODO - Use object pooling instead of instantiate/destroy. 
		// Might need to rework InventorySlot a bit, not sure. Or move the slot somewhere else? Not sure yet. 
		// Maybe just unparenting it from _inventoryContent will be enough. 

		// TODO - How to decide between Building and Crafting recipes? 
		// Might just make two scripts, UIBuilding and UICrafting instead of UIRecipes. 
		List<SORecipe> recipeList = BuildingOrCraftingMenu == BuildOrCraft.Build ?
			GameDataSO.InventoryDataSO.PossibleBuildingRecipes.ToList<SORecipe>() : 
			GameDataSO.InventoryDataSO.PossibleCraftingRecipes.ToList<SORecipe>();

		foreach (SORecipe recipeSO in recipeList)
		{
			Debug.Log($"Instantiating {recipeSO} in {BuildingOrCraftingMenu}ing menu.");
			GameObject slot = Instantiate(RecipeSlotPrefab, SlotsParent);
			slot.GetComponent<RecipeSlot>().SetupSlot(recipeSO);
		}
	}

	private void ClearSlots()
	{
		foreach (Transform slotTransform in SlotsParent)
		{
			Destroy(slotTransform.gameObject);
		}
	}
}