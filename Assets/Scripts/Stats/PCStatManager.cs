using System;
using System.Collections.Generic;
using UnityEngine;

// Put one on each PC. 
public class PCStatManager : MonoBehaviour
{
    public static event Action OnStatsChanged;

    [SerializeField]
    protected List<Stat> _stats;

    protected EquipmentManager _equipmentManager;

    public List<Stat> Stats { get { return _stats; } }

    protected void Start()
    {
        CalculateStatModifiers();

        // Have to get in Start, since it gets newed in EquipmentManager's OnEnable. 
        _equipmentManager = transform.parent.GetComponentInChildren<EquipmentManager>();
    }

    protected void OnEnable()
    {
        _equipmentManager.EquipmentSO.OnEquipmentChanged += CalculateStatModifiers;
        Stat.OnBaseValueChanged += CalculateStatModifiers;
    }

    protected void OnDisable()
    {
        _equipmentManager.EquipmentSO.OnEquipmentChanged -= CalculateStatModifiers;
        Stat.OnBaseValueChanged -= CalculateStatModifiers;
    }

    protected void CalculateStatModifiers()
    {
        // Keep the bonuses dictionary in EquipmentManager, and change it every time
        // equipment is changed. Then it's just ready and you can grab it whenever. 
        foreach (Stat stat in Stats)
        {
            stat.ClearModifiers();

            if (_equipmentManager.EquipmentBonuses.ContainsKey(stat.StatTypeSO))
            {
                stat.AddModifier(_equipmentManager.EquipmentBonuses[stat.StatTypeSO]);
            }
        }

        // TODO - Set up stats UI and UIStats script. Just show stats on the equipment UI. 
        // Heard by UIEquipment, updates UI. 
        // Heard by UIRecipes, gets new metRequirementsRecipes list. 
        OnStatsChanged?.Invoke();
    }
}