using System;
using System.Collections.Generic;
using UnityEngine;

// TODO - Make this a plain C# class, and pass the SOPCData in the constructor? No reason for it to be a MB.
// Maybe even combine it with Equipment class? 
// Where to new these three managers from? PCStateMachine? Rename it PCController or something? 
public class EquipmentManager : MonoBehaviour
{
    /// <summary>
    /// Heard by PlayerInventoryManager, adds item back into inventory. 
    /// </summary>
    public static event Action<SOEquipmentItem> OnUnequip;
    /// <summary>
    /// Heard by PCStatManager, recalculates stat modifiers. 
    /// </summary>
    public event Action OnEquipmentChanged;

    // To get equipment list. 
    private SOPCData _pcSO;

    public Dictionary<StatType, int> EquipmentBonuses { get; private set; } = new();

    private void OnEnable()
    {
        _pcSO = GetComponentInParent<PCController>().PCSO;
    }

    public void Equip(SOEquipmentItem equipmentItemSO)
    {
        // If there is something equipped in this slot, unequip it. 
        if (_pcSO.Equipment[equipmentItemSO.EquipmentType] != null)
        {
            SOEquipmentItem oldItem = _pcSO.Equipment[equipmentItemSO.EquipmentType];

            Unequip(oldItem);
        }

        // Add new item to EquipmentItems. 
        _pcSO.Equipment.Add(equipmentItemSO);

        AddEquipmentBonuses(equipmentItemSO);

        OnEquipmentChanged?.Invoke();
    }

    private void AddEquipmentBonuses(SOEquipmentItem equipmentItemSO)
    {
        foreach (EquipmentBonus equipmentBonus in equipmentItemSO.Bonuses)
        {
            if (EquipmentBonuses.ContainsKey(equipmentBonus.StatType))
            {
                EquipmentBonuses[equipmentBonus.StatType] += equipmentBonus.BonusAmount;
            }
            else
            {
                EquipmentBonuses.Add(equipmentBonus.StatType, equipmentBonus.BonusAmount);
            }
        }
    }

    public void Unequip(SOEquipmentItem equipmentItemSO)
    {
        // Remove old item from EquipmentItems. 
        _pcSO.Equipment.Remove(equipmentItemSO);

        RemoveEquipmentBonuses(equipmentItemSO);

        // UIEquipment (not yet, TODO ) and SOStatManager listen. 
        OnEquipmentChanged?.Invoke();

        OnUnequip?.Invoke(equipmentItemSO);
    }

    private void RemoveEquipmentBonuses(SOEquipmentItem equipmentItemSO)
    {
        foreach (EquipmentBonus equipmentBonus in equipmentItemSO.Bonuses)
        {
            if (EquipmentBonuses.ContainsKey(equipmentBonus.StatType))
            {
                EquipmentBonuses[equipmentBonus.StatType] -= equipmentBonus.BonusAmount;
                if (EquipmentBonuses[equipmentBonus.StatType] < 0)
                {
                    Debug.LogWarning($"{equipmentBonus.StatType}'s equipment bonus is below zero, this should never happen. ");
                    EquipmentBonuses.Remove(equipmentBonus.StatType);
                }
                else if (EquipmentBonuses[equipmentBonus.StatType] == 0)
                {
                    EquipmentBonuses.Remove(equipmentBonus.StatType);
                }
            }
            else
            {
                Debug.LogWarning($"Key {equipmentBonus.StatType} not found on _equipmentBonuses. This should never happen. ");
            }
        }
    }
}