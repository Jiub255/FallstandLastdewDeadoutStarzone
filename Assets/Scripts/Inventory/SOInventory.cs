using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO - Keep just the data here, and make an InventoryController class to do the Add/Remove/Contains? 
/// </summary>
[CreateAssetMenu(menuName = "Inventory/SOInventory", fileName = "New Inventory SO")]
public class SOInventory : ScriptableObject, IResettable/*, IGenericSOInventory where T : SOItem*/
{
    public event Action OnInventoryChanged;

    // Serialized for now just to see in inspector. 
    [field: SerializeField]
    public List<ItemAmount> ItemAmounts { get; private set; } = new();

    /// <summary>
    /// Called by InventoryController. 
    /// </summary>
    public void InventoryChanged()
    {
        OnInventoryChanged?.Invoke();
    }

    // FOR TESTING
    // Clear inventory when exiting play mode. 
    public void ResetOnExitPlayMode()
    {
//        ItemAmounts.Clear();
    }
}