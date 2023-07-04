using System;
using UnityEngine;


// TODO - Rework this for multiple PCs. 
public class StatManager : MonoBehaviour
{
    public static event Action OnStatsChanged;

    [SerializeField]
    private Stats _statsSO;
    [SerializeField]
    private Equipment _equipment;

    private void Start()
    {
        CalculateStatModifiers();

        // Have to get in Start, since it gets newed in EquipmentManager's OnEnable. 
        _equipment = transform.parent.GetComponentInChildren<EquipmentManager>().Equipment;
    }

    private void OnEnable()
    {
        _equipment.OnEquipmentChanged += CalculateStatModifiers;
        Stat.OnBaseValueChanged += CalculateStatModifiers;
    }

    private void OnDisable()
    {
        _equipment.OnEquipmentChanged -= CalculateStatModifiers;
        Stat.OnBaseValueChanged -= CalculateStatModifiers;
    }

    private void CalculateStatModifiers()
    {
        // Clear all modifiers on all stats. 
        foreach (Stat stat in _statsSO.StatSOs)
        {
            stat.ClearModifiers();
        }

        // Add each bonus.
        foreach (SOEquipmentItem equipmentItem in _equipment.EquipmentItems)
        {
            foreach (EquipmentBonus bonus in equipmentItem.Bonuses)
            {
                bonus.StatSO.AddModifier(bonus.BonusAmount);
            }
        }

        // Heard by UIStats and PlayerMeleeAttack. 
        OnStatsChanged?.Invoke();
    }
}