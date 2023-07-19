using System;
using UnityEngine;

// Put one on each PC. 
public class PCStatManager : MonoBehaviour
{
    public static event Action OnStatsChanged;

    [SerializeField]
    private SOPC _pcSO;

    private EquipmentManager _equipmentManager;

    // Just making a getter for PCSelector to use have access to the SOPC from the PC instance. 
    public SOPC PCSO { get { return _pcSO; } }

    protected void OnEnable()
    {
        _equipmentManager = transform.parent.GetComponentInChildren<EquipmentManager>();
        _equipmentManager.OnEquipmentChanged += CalculateStatModifiers;
        Stat.OnBaseValueChanged += CalculateStatModifiers;

        CalculateStatModifiers();
    }

    protected void OnDisable()
    {
        _equipmentManager.OnEquipmentChanged -= CalculateStatModifiers;
        Stat.OnBaseValueChanged -= CalculateStatModifiers;
    }

    protected void CalculateStatModifiers()
    {
        // Keep the bonuses dictionary in EquipmentManager, and change it every time
        // equipment is changed. Then it's just ready and you can grab it whenever. 
        foreach (Stat stat in _pcSO.Stats)
        {
            stat.ClearModifiers();

            if (_equipmentManager.EquipmentBonuses.ContainsKey(stat.StatTypeSO))
            {
                stat.AddModifier(_equipmentManager.EquipmentBonuses[stat.StatTypeSO]);
            }
        }

        // TODO - Set up stats UI and UIStats script. Just show stats on the equipment UI. 
        // TODO - Heard by UIEquipment, updates UI. 
        // Heard by UIRecipes, gets new metRequirementsRecipes list. 
        OnStatsChanged?.Invoke();
    }
}