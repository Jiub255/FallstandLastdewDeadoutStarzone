using System;
using UnityEngine;

// SOCraftingItem, SOEquipmentItem, and SOUsableItem all inherit from this. 
public abstract class SOItem : SORecipe
{
    public static event Action<SOItem> OnRemoveItem;
    public static event Action<SOItem> OnAddItem;
    public static event Action<SOItem> OnSelectItem;

    // Called by clicking on Inventory Slot. 
    public abstract void OnClickInventory();

    // Called by button on Recipe Slot. 
    public override void OnClickRecipe()
    {
        // InventoryManager hears this, adds item to inventory. 
        OnSelectItem?.Invoke(this);
    }

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