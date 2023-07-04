using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Put this on each canvas object (usable, crafting, equipment). 
public class UIInventory : MonoBehaviour
{
	// Will this work? Can you set it as a SOInventory<SOUsableItem> in the inspector? 
	// Use interfaces somehow? To constrain the generic type? 
	// Not using generics or interfaces. Just have different item types derive from SOItem, and create corresponding SOInventorys, 
	// then InventoryManager handles putting the items in the right inventory. No need for all that fancy stuff. 
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