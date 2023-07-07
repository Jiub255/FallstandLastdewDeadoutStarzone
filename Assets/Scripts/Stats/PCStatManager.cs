using System;
using System.Collections.Generic;
using UnityEngine;

// Put one on each PC. 
public class PCStatManager : MonoBehaviour
{
    public static event Action OnStatsChanged;

    [SerializeField]
    protected List<Stat> _stats;

    // Just serialized to see in inspector for testing. 
    [SerializeField, Header("Only Serialized For Testing")]
    protected Equipment _equipment;

    public List<Stat> Stats { get { return _stats; } }

    protected void Start()
    {
        CalculateStatModifiers();

        // Have to get in Start, since it gets newed in EquipmentManager's OnEnable. 
        _equipment = transform.parent.GetComponentInChildren<EquipmentManager>().Equipment;
    }

    protected void OnEnable()
    {
        _equipment.OnEquipmentChanged += CalculateStatModifiers;
        Stat.OnBaseValueChanged += CalculateStatModifiers;
    }

    protected void OnDisable()
    {
        _equipment.OnEquipmentChanged -= CalculateStatModifiers;
        Stat.OnBaseValueChanged -= CalculateStatModifiers;
    }

    protected void CalculateStatModifiers()
    {
        // TODO - Can this be done better? Without three nested foreach loops? 
        foreach (Stat stat in _stats/*.StatsList*/)
        {
            stat.ClearModifiers();

            foreach (SOEquipmentItem equipmentItem in _equipment.EquipmentItems)
            {
                foreach (EquipmentBonus bonus in equipmentItem.Bonuses)
                {
                    if (bonus.StatTypeSO == stat.StatTypeSO)
                    {
                        stat.AddModifier(bonus.BonusAmount);
                    }
                }
            }
        }

        // Heard by UIStats and PlayerMeleeAttack. 
        OnStatsChanged?.Invoke();
    }
}