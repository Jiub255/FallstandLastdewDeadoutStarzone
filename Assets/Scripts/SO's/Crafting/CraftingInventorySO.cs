using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingInventorySO : InventorySO
{
	public List<CraftingItemAmount> CraftingItemAmounts = new List<CraftingItemAmount>();

    public CraftingItemAmount GetCraftingItemAmountFromItemSO(ItemSO itemSO)
    {
        foreach (CraftingItemAmount itemAmount in CraftingItemAmounts)
        {
            if (itemAmount.CraftingItemSO == itemSO)
            {
                return itemAmount;
            }
        }

        return null;
    }
}