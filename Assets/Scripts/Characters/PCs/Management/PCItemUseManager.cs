using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Make this class in PCManager, and use it to subscribe to all the equipment and usable item effect events. <br/>
/// TODO - Eventually hold all item SOs in InventoryDataSO, and go through them here to subscribe to events without them having to be static events. 
/// </summary>
public class PCItemUseManager
{
//    private Dictionary<SOPCData, PCController> PCControllerDict { get; }
    private SOPCData CurrentMenuPC { get; }
//    private SOInventoryData InventoryDataSO { get; }

    /// <summary>
    /// TODO - Will passing the SOPCData by ref work? I want the reference to always point to the same thing as it does in PCManager. 
    /// </summary>
    /// <param name="currentMenuPC"></param>
    public PCItemUseManager(/*Dictionary<SOPCData, PCController> pcControllerDict, */ref SOPCData currentMenuPC)
    {
//        PCControllerDict = pcControllerDict;
        CurrentMenuPC = currentMenuPC;
/*        InventoryDataSO = inventoryDataSO;

        // Equipment 
        foreach (SOEquipmentItem equipmentItemSO in InventoryDataSO.EquipmentInventorySO.ItemAmounts.Select(itemAmount => (SOEquipmentItem)itemAmount.ItemSO))
        {
            equipmentItemSO.OnEquip += () => HandleEquip(equipmentItemSO);
            equipmentItemSO.OnUnequip += () => HandleUnequip(equipmentItemSO);
        }

        // Usable items 
        // How to do this? Each effect has its own event to subscribe to a particular function. Maybe just do static events instead. 
        foreach (SOUsableItem usableItemSO in InventoryDataSO.UsableInventorySO.ItemAmounts.Select(itemAmount => (SOUsableItem)itemAmount.ItemSO))
        {
            usableItemSO.OnClickInventory
        }*/

        SOEquipmentItem.OnEquip += HandleEquip;
        SOEquipmentItem.OnUnequip += HandleUnequip;

        // Usable items 
        SORelievePain.OnRelievePainEffect += HandleRelievePainEffect;
    }

    public void OnDisable()
    {
/*        foreach (SOEquipmentItem equipmentItemSO in InventoryDataSO.EquipmentInventorySO.ItemAmounts.Select(itemAmount => (SOEquipmentItem)itemAmount.ItemSO))
        {
            equipmentItemSO.OnEquip += () => HandleEquip(equipmentItemSO);
            equipmentItemSO.OnUnequip += () => HandleUnequip(equipmentItemSO);
        }*/

        // Equipment 
        SOEquipmentItem.OnEquip -= HandleEquip;
        SOEquipmentItem.OnUnequip -= HandleUnequip;

        // Usable items 
        SORelievePain.OnRelievePainEffect -= HandleRelievePainEffect;
    }

    private void HandleEquip(SOEquipmentItem item)
    {
        if (CurrentMenuPC != null)
//            PCControllerDict[CurrentMenuPC].EquipmentManager.Equip(item);
            CurrentMenuPC.PCController.EquipmentManager.Equip(item);
        else
            Debug.LogWarning("CurrentMenuPC is null in PCManager. This should never happen, should get set on scene load. ");
    }

    private void HandleUnequip(SOEquipmentItem item)
    {
        if (CurrentMenuPC != null)
//            PCControllerDict[CurrentMenuPC].EquipmentManager.Unequip(item);
            CurrentMenuPC.PCController.EquipmentManager.Unequip(item);
        else
            Debug.LogWarning("CurrentMenuPC is null in PCManager. This should never happen, should get set on scene load. ");
    }

    private void HandleRelievePainEffect(int amount, float duration)
    {
        if (CurrentMenuPC != null)
//            PCControllerDict[CurrentMenuPC].PainInjuryManager.TemporarilyRelievePain(amount, duration);
            CurrentMenuPC.PCController.PainInjuryManager.TemporarilyRelievePain(amount, duration);
        else
            Debug.LogWarning("CurrentMenuPC is null in PCManager. This should never happen, should get set on scene load. ");
    }
}