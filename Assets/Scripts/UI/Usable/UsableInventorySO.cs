using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Usable Inventory SO", menuName = "Scriptable Object/Usable/Usable Inventory SO")]
public class UsableInventorySO : ScriptableObject, IInventory
{
    public List<UsableItemAmount> UsableItemAmounts = new List<UsableItemAmount>();

    public UsableItemAmount GetUsableItemAmountFromItemSO(UsableItemSO usableItemSO)
    {
        foreach (UsableItemAmount usableItemAmount in UsableItemAmounts)
        {
            if (usableItemAmount.UsableItemSO == usableItemSO)
            {
                return usableItemAmount;
            }
        }

        return null;
    }

    /*	public void Add(ItemAmount itemAmount)
        {
            if (itemAmount.ItemSO.GetType() == typeof(UsableItemSO))
            {
                ItemAmount listItemAmount = GetItemAmountFromItemSO(itemAmount.ItemSO);

                if (listItemAmount != null)
                {
                    listItemAmount.Amount += itemAmount.Amount;
                }
                else
                {
                    ItemAmounts.Add(itemAmount);
                }
            }
            else
            {
                Debug.LogWarning("Couldn't Add: Wrong Item Type");
            }
        }

        public void Remove(ItemAmount itemAmount)
        {
            if (itemAmount.ItemSO.GetType() == typeof(UsableItemSO))
            {
                ItemAmount listItemAmount = GetItemAmountFromItemSO(itemAmount.ItemSO);

                if (listItemAmount != null)
                {
                    listItemAmount.Amount -= itemAmount.Amount;
                    if (listItemAmount.Amount < 0)
                    {
                        ItemAmounts.Remove(listItemAmount);
                    }
                }
                else
                {
                    Debug.LogWarning("Couldn't Remove: Item Not In List");
                }
            }
            else
            {
                Debug.LogWarning("Couldn't Remove: Wrong Item Type");
            }
        }*/
}