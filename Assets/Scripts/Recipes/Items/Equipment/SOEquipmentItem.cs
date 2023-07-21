using System;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor
}

//[CreateAssetMenu(fileName = "New Equipment Item SO", menuName = "Items/SOEquipmentItem")]
public class SOEquipmentItem : SOItem
{
    // int is CurrentMenuPC's InstanceID. 
    public static event Action<SOEquipmentItem> OnEquip;
    public static event Action<SOEquipmentItem> OnUnequip;


    // TODO - Have each armor type inherit this class, or use the enum? 
    // If it's just gonna be armor and weapons then yes. 
    // Or, with multiple armor types, could do SOWeaponItem and SOArmorItem and have an ArmorType enum.
    // Then weapon can have attack and weapon range fields, and armor can have defense. Might not even need EquipmentBonus class, just use an int and code. 
    // Weapons only affect attack and armor only affects defense, the equipment bonus bit is good for more complicated stuff, but not needed here. 
    // OR, just do each equipment type as its own subclass. It'd be like the equipmentType enum, but hold data too. 
    public EquipmentType EquipmentType;

    public List<EquipmentBonus> Bonuses;

//    public GameObject EquipmentItemPrefab;

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