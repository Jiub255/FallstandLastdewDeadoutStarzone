using System.Collections.Generic;
using UnityEngine;

// TODO: Redo this for UsableItemSO's? Don't want to show all itemSO's in inventory. 
// Maybe rename UsableItemSO back to InventoryItemSO? Either way be consistent with naming: usable vs. inventory vs. all items. 
[CreateAssetMenu(fileName = "New Inventory SO", menuName = "Scriptable Object/Inventory/Inventory SO")]
public class InventorySO : ScriptableObject
{
    public List<OLDItemAmount> ItemAmounts = new List<OLDItemAmount>();

    public OLDItemAmount GetItemAmountFromItemSO(ItemSO itemSO)
    {
        foreach (OLDItemAmount itemAmount in ItemAmounts)
        {
            if (itemAmount.ItemSO == itemSO)
            {
                return itemAmount;
            }
        }

        return null;
    }
}