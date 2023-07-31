using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make this class in PCManager, and use it to subscribe to all the equipment and usable item effect events. 
/// TODO - Eventually hold all item SOs in InventoryDataSO, and go through them here to subscribe to events without them having to be static events. <br/>
/// Also, maybe put this on a higher level so it can access inv and build data better? Add player level right below game and put it there? 
/// </summary>
public class PCItemUseManager
{
    private Dictionary<SOPCData, PCController> PCControllerDict { get; }
    private SOPCData CurrentMenuPC { get; }

    public PCItemUseManager(Dictionary<SOPCData, PCController> pcControllerDict, SOPCData currentMenuPC)
    {
        PCControllerDict = pcControllerDict;
        CurrentMenuPC = currentMenuPC;

        // Equipment 
        SOEquipmentItem.OnEquip += HandleEquip;
        SOEquipmentItem.OnUnequip += HandleUnequip;

        // Usable items 
        SORelievePain.OnRelievePainEffect += HandleRelievePainEffect;
    }

    public void OnDisable()
    {
        // Equipment 
        SOEquipmentItem.OnEquip -= HandleEquip;
        SOEquipmentItem.OnUnequip -= HandleUnequip;

        // Usable items 
        SORelievePain.OnRelievePainEffect -= HandleRelievePainEffect;
    }

    private void HandleEquip(SOEquipmentItem item)
    {
        if (CurrentMenuPC != null)
            PCControllerDict[CurrentMenuPC].EquipmentManager.Equip(item);
        else
            Debug.LogWarning("CurrentMenuPC is null in PCManager. This should never happen, should get set on scene load. ");
    }

    private void HandleUnequip(SOEquipmentItem item)
    {
        if (CurrentMenuPC != null)
            PCControllerDict[CurrentMenuPC].EquipmentManager.Unequip(item);
        else
            Debug.LogWarning("CurrentMenuPC is null in PCManager. This should never happen, should get set on scene load. ");
    }

    private void HandleRelievePainEffect(int amount, float duration)
    {
        if (CurrentMenuPC != null)
            PCControllerDict[CurrentMenuPC].PainInjuryManager.TemporarilyRelievePain(amount, duration);
        else
            Debug.LogWarning("CurrentMenuPC is null in PCManager. This should never happen, should get set on scene load. ");
    }
}