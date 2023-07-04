using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Inventory SO", menuName = "Scriptable Object/Crafting/Crafting Inventory SO")]
public class CraftingInventorySO : ScriptableObject, IInventory
{
	public List<CraftingItemAmount> CraftingItemAmounts = new List<CraftingItemAmount>();

    public CraftingItemAmount GetCraftingItemAmountFromItemSO(CraftingItemSO craftingItemSO)
    {
        foreach (CraftingItemAmount craftingItemAmount in CraftingItemAmounts)
        {
            if (craftingItemAmount.CraftingItemSO == craftingItemSO)
            {
                return craftingItemAmount;
            }
        }

        return null;
    }
}