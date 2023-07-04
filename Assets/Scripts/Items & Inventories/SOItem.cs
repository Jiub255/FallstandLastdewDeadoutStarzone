using System;
using UnityEngine;

// SOCraftingItem, SOEquipmentItem, and SOUsableItem all inherit from this. 
public abstract class SOItem : ScriptableObject
{
    public static event Action<SOItem> OnRemoveItem;
    public static event Action<SOItem> OnAddItem;

    public string Description = "Enter Item Description";
	public Sprite Icon;

	// Called by clicking on the slot button from the inventory menu. 
	// SOCraftingItem will have a different method for clicking from the crafting window. 
	public abstract void OnClickFromInventory();

    // Heard by PlayerInventoryManager. 
    protected void AddToInventory()
    {
        OnAddItem?.Invoke(this);
    }

    // Heard by PlayerInventoryManager. 
    protected void RemoveFromInventory()
    {
        OnRemoveItem?.Invoke(this);
    }
}