using UnityEngine;

// Put this on the Crafting and Building canvases. 
public class UIRecipes : MonoBehaviour
{
	[SerializeField]
	private RecipeList _recipeList;
	[SerializeField]
	private GameObject _recipeSlotPrefab;
	[SerializeField]
	private Transform _slotsParent;

	private void OnEnable()
	{
		_recipeList.PopulateList();

		SetupRecipeSlots();
	}

	// TODO - Gray out the buildings that you don't have enough materials to build. 
	// Will have to call this method every time you use some materials to build something. 
	public void SetupRecipeSlots()
	{
//		Debug.Log("SetupRecipeSlots called. ");
		ClearSlots();

		// TODO - Use object pooling instead of instantiate/destroy. 
		// Might need to rework InventorySlot a bit, not sure. Or move the slot somewhere else? Not sure yet. 
		// Maybe just unparenting it from _inventoryContent will be enough. 
		foreach (SORecipe recipeSO in _recipeList.Recipes)
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