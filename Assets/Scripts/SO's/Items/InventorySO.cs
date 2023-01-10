using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySO", menuName = "Items/InventorySO")]
public class InventorySO : ScriptableObject
{
    public List<ItemAmount> itemAmounts = new List<ItemAmount>();

    public ItemAmount ItemToItemAmount(ItemSO item)
    {
        foreach (ItemAmount itemAmount in itemAmounts)
        {
            if (itemAmount.item == item)
            {
                return itemAmount;
            }
        }

        return null;
    }
}

// TODO: Put amount in ItemSO class, get rid of this ItemAmount class.
// Only needed it for another game because multiple characters used the same items in their inventories.
// Not an issue in this game.
[System.Serializable]
public class ItemAmount
{
    public ItemSO item;
    public int amount;

    public ItemAmount(ItemSO item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}