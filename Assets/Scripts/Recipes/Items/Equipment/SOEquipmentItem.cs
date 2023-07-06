using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item SO", menuName = "Items/SOEquipmentItem")]
public class SOEquipmentItem : SOItem
{
    public static event Action<SOEquipmentItem> OnEquip;
    public static event Action<SOEquipmentItem> OnUnequip;

    public SOEquipmentType EquipmentType;

    public List<EquipmentBonus> Bonuses;

    public GameObject EquipmentItemPrefab;

    // Equip the item. Send signal to equipment and inventory managers. 
    // Called by "use" button on inventory slot prefab. 
    public override void OnClickInventory()
    {
        Debug.Log($"Clicked on equipment item {name}"); 

        // Sends signal to EquipmentManager. 
        OnEquip?.Invoke(this);

        // Sends signal to PlayerInventoryManager. 
        RemoveFromInventory();

        Debug.Log($"Equipped {name}");
    }

    // Unequip the item. Send signal to equipment and inventory managers. 
    // Called by "unequip" button on equipment slot prefab. 
    public void Unequip()
    {
        // Sends signal to EquipmentManager. 
        OnUnequip?.Invoke(this);

        // Sends signal to PlayerInventoryManager. 
        AddToInventory();

        Debug.Log($"Unequipped {name}");
    }
}