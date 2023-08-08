using UnityEngine;

// Put this on each canvas object (usable, crafting, equipment). 
public class UIInventory : MonoBehaviour
{
	// Different item types derive from SOItem, and create corresponding SOInventorys, 
	// then InventoryManager handles putting the items in the right inventory.
	/// <summary>
	/// Just using to get GameData for now, probably going to add it to the GameManager eventually and have it pass that down in the constructor. 
	/// </summary>
	[SerializeField]
	private SOInventory _inventorySO;
	[SerializeField]
	private GameObject _inventorySlotPrefab;
	[SerializeField]
	private Transform _slotsParent;

	private SOInventory InventorySO { get { return _inventorySO; } }
	private GameObject InventorySlotPrefab { get { return _inventorySlotPrefab; } }
	private Transform SlotsParent { get { return _slotsParent; } }

    private void Start()
    {
		SetupInventorySlots();

		InventorySO.InventoryController.OnInventoryChanged += SetupInventorySlots;        
    }

    private void OnDisable()
	{
		InventorySO.InventoryController.OnInventoryChanged -= SetupInventorySlots;
	}

	private void SetupInventorySlots()
	{
		ClearSlots();

		// TODO - Use object pooling instead of instantiate/destroy. 
		// Might need to rework InventorySlot a bit, not sure. Or move the slot somewhere else? Not sure yet. 
		// Maybe just unparenting it from _inventoryContent will be enough. 
		foreach (ItemAmount itemAmount in InventorySO.ItemAmounts)
		{
			GameObject slot = Instantiate(InventorySlotPrefab, SlotsParent);
			slot.GetComponent<InventorySlot>().SetupSlot(itemAmount);
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