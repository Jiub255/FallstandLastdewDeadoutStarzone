﻿using UnityEngine;

public class InventoryUI : UIRefresher
{
    [SerializeField]
    private InventorySO inventorySO;

    private void OnEnable()
    {
        UIManager.onOpenedMenu += PopulateInventory;
    }

    private void OnDisable()
    {
        UIManager.onOpenedMenu -= PopulateInventory;
    }

    public override void PopulateInventory()
    {
        base.PopulateInventory();

        foreach (ItemAmount itemAmount in inventorySO.itemAmounts)
        {
            GameObject slotInstance = Instantiate(slotPrefab, content);
            
            slotInstance.transform.GetComponent<InventorySlot>().SetupSlot(itemAmount);
        }
    }
}