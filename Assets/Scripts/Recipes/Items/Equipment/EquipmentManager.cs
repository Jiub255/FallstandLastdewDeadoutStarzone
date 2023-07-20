using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static event Action<SOEquipmentItem> OnUnequip;
    public event Action OnEquipmentChanged;

    // So StatManager can get equipment list. 
    private SOPC _pcSO;

    public Dictionary<StatType, int> EquipmentBonuses { get; private set; } = new();

    private void OnEnable()
    {
        SOEquipmentItem.OnEquip += Equip;
        SOEquipmentItem.OnUnequip += Unequip;
        _pcSO = GetComponentInParent<PCStateMachine>().PCSO;
    }

    private void OnDisable()
    {
        SOEquipmentItem.OnEquip -= Equip;
        SOEquipmentItem.OnUnequip -= Unequip;
    }

    // TODO - As is, this will equip the item on every PC. How to make it only the MenuSelected one?
    // Could pass an id parameter in the static event and have all EquipmentManagers check. 
    // But that seems ugly and there's probably a better way. 
    // Could just subscribe only when selected, so only the selected instance gets the event. 
    // Then unsubscribe when deselected (and still OnDisable too). 
    public void Equip(SOEquipmentItem equipmentItemSO)
    {
//        PCSO.Equip(equipmentItemSO);

        EquipmentType type = equipmentItemSO.EquipmentType;

        // If there is something equipped in this slot, unequip it. 
        for (int i = 0; i < _pcSO.EquipmentItems.Count; i++)
        {
            if (type == _pcSO.EquipmentItems[i].EquipmentType)
            {
                SOEquipmentItem oldItem = _pcSO.EquipmentItems[i];

                Unequip(oldItem);
            }
        }

        // Add new item to EquipmentItems. 
        _pcSO.EquipmentItems.Add(equipmentItemSO);

        // UIEquipment and StatManager listen. 
        OnEquipmentChanged?.Invoke();

        AddEquipmentBonuses(equipmentItemSO);
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
//        PCSO.Unequip(equipmentItemSO);

        // Remove old item from EquipmentItems. 
        _pcSO.EquipmentItems.Remove(equipmentItemSO);

        // UIEquipment (not yet, TODO ) and SOStatManager listen. 
        OnEquipmentChanged?.Invoke();

        // InventoryManager listens.
        OnUnequip?.Invoke(equipmentItemSO);

        RemoveEquipmentBonuses(equipmentItemSO);
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