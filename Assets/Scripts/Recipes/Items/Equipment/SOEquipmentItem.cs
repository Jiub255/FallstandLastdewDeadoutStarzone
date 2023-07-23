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
    // Keep Equipment type, so Equipment can use it as an index. Have SOWeaponItem extend SOEquipmentItem so it can have a guaranteed attack stat
    // (can also have bonuses), and an attack range stat (some small number for melee/unarmed). Also have SOArmorItem with a defense stat and maybe more. 
    // Otherwise, keep Equipment class as an "indexed by type" List of SOEquipmentItems and have the manager only allow one of each type. 

    [SerializeField, Header("-------- General Equipment Data --------")]
    protected EquipmentType _equipmentType;
    [SerializeField]
    protected List<EquipmentBonus> _bonuses;

    // Overridden by SOWeaponItem with { return EquipmentType.Weapon; }
    public virtual EquipmentType EquipmentType { get { return _equipmentType; } }
    public List<EquipmentBonus> Bonuses { get { return _bonuses; } }

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