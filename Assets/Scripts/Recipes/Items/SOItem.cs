using System;
using UnityEngine;

// SOCraftingItem, SOEquipmentItem, and SOUsableItem all inherit from this. 
public abstract class SOItem : SORecipe
{
    /// <summary>
    /// Heard by PlayerInventoryManager. 
    /// </summary>
    public static event Action<SOItem> OnRemoveItem;
    /// <summary>
    /// Heard by PlayerInventoryManager. 
    /// </summary>
    public static event Action<SOItem> OnAddItem;
    /// <summary>
    /// PlayerInventoryManager hears this, adds item to inventory, and removes the crafting items necessary to create item. 
    /// </summary>
    public static event Action<SOItem> OnSelectItem;

    /// <summary>
    /// Called by button on Inventory Slot. 
    /// </summary>
    public abstract void OnClickInventory();

    /// <summary>
    /// Called by button on Recipe Slot. 
    /// </summary>
    public override void OnClickRecipe()
    {
        OnSelectItem?.Invoke(this);
    }

    protected void AddToInventory()
    {
        OnAddItem?.Invoke(this);
    }

    protected void RemoveFromInventory()
    {
        OnRemoveItem?.Invoke(this);
    }
}