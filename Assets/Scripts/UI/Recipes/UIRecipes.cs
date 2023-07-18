using System;
using System.Collections.Generic;
using UnityEngine;

// Put this on the Crafting and Building canvases. 
public class UIRecipes : MonoBehaviour
{
	public static event Func<List<SORecipe>, List<SORecipe>> OnGetMetStatRequirementsRecipes;
	public static event Func<List<SORecipe>, List<SORecipe>> OnGetHaveEnoughItemsRecipes;

	[SerializeField]
	private RecipeList _recipeList;
	[SerializeField]
	private GameObject _recipeSlotPrefab;
	[SerializeField]
	private Transform _slotsParent;

	protected List<SORecipe> _metStatRequirementsRecipes;
	protected List<SORecipe> _haveEnoughItemsRecipes;

	public virtual void OnEnable()
	{
		// Call this whenever stats change. 
		GetRecipeLists();

		// Toggle between showing _metRequirementsRecipes and _haveEnoughItemsRecipes by calling SetupRecipeSlots and passing whichever list. 
		SetupRecipeSlots(_haveEnoughItemsRecipes);

		PlayerInventoryManager.OnPlayerInventoryChanged += GetHaveEnoughItemsRecipes;
		PCStatManager.OnStatsChanged += GetRecipeLists;
	}

    private void OnDisable()
    {
		PlayerInventoryManager.OnPlayerInventoryChanged -= GetHaveEnoughItemsRecipes;
		PCStatManager.OnStatsChanged -= GetRecipeLists;
	}

	private void GetRecipeLists()
    {
		GetMetStatRequirementsRecipes();
		GetHaveEnoughItemsRecipes();
	}

	private void GetMetStatRequirementsRecipes()
    {
		// StatManager listens, sends back all recipes that you meet the stat requirements for. 
	    _metStatRequirementsRecipes = OnGetMetStatRequirementsRecipes(_recipeList.GetAllRecipes());
    }

	private void GetHaveEnoughItemsRecipes()
	{
		// PlayerInventoryManager listens, sends back all recipes that you have enough items to craft/build. 
		_haveEnoughItemsRecipes = OnGetHaveEnoughItemsRecipes(_metStatRequirementsRecipes);
	}

    // TODO - Gray out the buildings/items that you don't have enough materials to build. 
    // Will have to call this method every time you use some materials to build something. 
    public void SetupRecipeSlots(List<SORecipe> possibleRecipes)
	{
//		Debug.Log("SetupRecipeSlots called. ");
		ClearSlots();

		// TODO - Use object pooling instead of instantiate/destroy. 
		// Might need to rework InventorySlot a bit, not sure. Or move the slot somewhere else? Not sure yet. 
		// Maybe just unparenting it from _inventoryContent will be enough. 
		foreach (SORecipe recipeSO in possibleRecipes)
		{
			GameObject slot = Instantiate(_recipeSlotPrefab, _slotsParent);
			slot.GetComponent<RecipeSlot>().SetupSlot(recipeSO);
		}
	}

	private void ClearSlots()
	{
		foreach (Transform slotTransform in _slotsParent)
		{
			Destroy(slotTransform.gameObject);
		}
	}
}