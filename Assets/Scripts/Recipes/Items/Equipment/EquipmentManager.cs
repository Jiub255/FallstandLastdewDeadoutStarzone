using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField]
    private Transform _weaponParent;
    [SerializeField]
    private SOEquipmentType _weaponEquipmentType;

    // So StatManager can get equipment list. 
    public Equipment Equipment { get; private set; }

    public Dictionary<SOStatType, int> EquipmentBonuses { get; private set; } = new();

    private void OnEnable()
    {
        SOEquipmentItem.OnEquip += Equip;
        SOEquipmentItem.OnUnequip += Unequip;

        Equipment = new();
    }

    private void OnDisable()
    {
        SOEquipmentItem.OnEquip -= Equip;
        SOEquipmentItem.OnUnequip -= Unequip;
    }

    public void Equip(SOEquipmentItem equipmentItemSO)
    {
        Equipment.Equip(equipmentItemSO);

        AddEquipmentBonuses(equipmentItemSO);

/*        if (newItem.EquipmentType == _weaponEquipmentType)
        {
            // Destroy old weapon object. 
            Destroy(_weaponParent.GetChild(0).gameObject);

            // Instantiate new weapon object
            GameObject weapon = Instantiate(newItem.EquipmentItemPrefab, _weaponParent);
            weapon.transform.localPosition = Vector3.zero;
        }*/
    }

    private void AddEquipmentBonuses(SOEquipmentItem equipmentItemSO)
    {
        foreach (EquipmentBonus equipmentBonus in equipmentItemSO.Bonuses)
        {
            if (EquipmentBonuses.ContainsKey(equipmentBonus.StatTypeSO))
            {
                EquipmentBonuses[equipmentBonus.StatTypeSO] += equipmentBonus.BonusAmount;
            }
            else
            {
                EquipmentBonuses.Add(equipmentBonus.StatTypeSO, equipmentBonus.BonusAmount);
            }
        }
    }

    public void Unequip(SOEquipmentItem equipmentItemSO)
    {
        Equipment.Unequip(equipmentItemSO);

        RemoveEquipmentBonuses(equipmentItemSO);

        /*        // Destroy old weapon object. 
                Destroy(_weaponParent.GetChild(0).gameObject);*/
    }

    private void RemoveEquipmentBonuses(SOEquipmentItem equipmentItemSO)
    {
        foreach (EquipmentBonus equipmentBonus in equipmentItemSO.Bonuses)
        {
            if (EquipmentBonuses.ContainsKey(equipmentBonus.StatTypeSO))
            {
                EquipmentBonuses[equipmentBonus.StatTypeSO] -= equipmentBonus.BonusAmount;
                if (EquipmentBonuses[equipmentBonus.StatTypeSO] < 0)
                {
                    Debug.LogWarning($"{equipmentBonus.StatTypeSO}'s equipment bonus is below zero, this should never happen. ");
                    EquipmentBonuses.Remove(equipmentBonus.StatTypeSO);
                }
                else if (EquipmentBonuses[equipmentBonus.StatTypeSO] == 0)
                {
                    EquipmentBonuses.Remove(equipmentBonus.StatTypeSO);
                }
            }
            else
            {
                Debug.LogWarning($"Key {equipmentBonus.StatTypeSO} not found on _equipmentBonuses. This should never happen. ");
            }
        }
    }
}