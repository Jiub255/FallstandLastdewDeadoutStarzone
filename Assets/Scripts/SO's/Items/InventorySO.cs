using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySO", menuName = "Items/InventorySO")]
public class InventorySO : ScriptableObject
{
    public List<ItemAmount> itemAmounts = new List<ItemAmount>();

    public ItemAmount ItemToItemAmount(Item item)
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

[System.Serializable]
public class ItemAmount
{
    public Item item;
    public int amount;

    public ItemAmount (Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}