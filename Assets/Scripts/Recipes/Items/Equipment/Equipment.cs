using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment
{
    [SerializeField]
    private List<SOEquipmentItem> _equipmentItems;

    public List<SOEquipmentItem> EquipmentItems { get { return _equipmentItems; } set { _equipmentItems = value; } }

    public Equipment()
    {
        _equipmentItems = new();
    }

    public SOEquipmentItem this[EquipmentType equipmentType]
    {
        get
        {
            foreach (SOEquipmentItem item in _equipmentItems)
            {
                if (item.EquipmentType == equipmentType)
                {
                    return item;
                }
            }

            Debug.LogWarning($"No equipment of type {equipmentType} found in EquipmentItems.");
            return null;
        }
    }

    public SOWeaponItem Weapon()
    {
        return (SOWeaponItem)this[EquipmentType.Weapon];
    }

    public void Add(SOEquipmentItem equipmentItem)
    {
        _equipmentItems.Add(equipmentItem);
    }

    public void Remove(SOEquipmentItem equipmentItem)
    {
        _equipmentItems.Remove(equipmentItem);
    }
}