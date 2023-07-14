using UnityEngine;

// Put this on each canvas object (usable, crafting, equipment). 
public class UIInventory : MonoBehaviour
{
	// Different item types derive from SOItem, and create corresponding SOInventorys, 
	// then InventoryManager handles putting the items in the right inventory.
	[SerializeField]
	private SOInventory _inventorySO;
	[SerializeField]
	private GameObject _inventorySlotPrefab;
	[SerializeField]
	private Transform _slotsParent;

	private void OnEnable()
	{
		SetupInventorySlots();

		_inventorySO.OnInventoryChanged += SetupInventorySlots;
//		MenuController.OnOpenInventory += SetupInventorySlots;
	}

	private void OnDisable()
	{
		_inventorySO.OnInventoryChanged -= SetupInventorySlots;
//		MenuController.OnOpenInventory -= SetupInventorySlots;
	}

	private void SetupInventorySlots()
	{
//		Debug.Log("SetupInventorySlots called. ");
		ClearSlots();

		// TODO - Use object pooling instead of instantiate/destroy. 
		// Might need to rework InventorySlot a bit, not sure. Or move the slot somewhere else? Not sure yet. 
		// Maybe just unparenting it from _inventoryContent will be enough. 
		foreach (ItemAmount itemAmount in _inventorySO.ItemAmounts)
		{
			GameObject slot = Instantiate(_inventorySlotPrefab, _slotsParent);
			slot.GetComponent<InventorySlot>().SetupSlot(itemAmount.ItemSO, itemAmount.Amount);
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