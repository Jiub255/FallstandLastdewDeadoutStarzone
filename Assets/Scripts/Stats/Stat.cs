using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Attack,
    Defense, 
    Scavenging, 
    Medical, 
    Engineering, 
    Farming
}

[Serializable]
public class Stat
{
    /// <summary>
    /// Heard by PCStatManager, recalculates stat modifiers. 
    /// </summary>
    /// <remarks>
    /// How to know which PC this affects? Could new these in SOPCData (from PCStatManager) based on presets in SOPCSharedData, and pass in the 
    /// PCStatManager in the constructor. Then the stat could call CalculateModdedValues or whatever itself. 
    /// </remarks>
//    public static event Action OnBaseValueChanged;

    [SerializeField]
    private StatType _statType;
    [SerializeField]
    private int _baseValue;

    private List<int> _modifiers;
    private int _moddedValue;
    private PCStatManager _pcStatManager;

    public StatType StatType { get { return _statType; } }
    public int ModdedValue { get { return _moddedValue; } }

    public Stat(StatType statType, int baseValue, PCStatManager pcStatManager)
    {
        _statType = statType;
        _baseValue = baseValue;
        _pcStatManager = pcStatManager;
    }

    private void CalculateModdedValue()
    {
        int finalValue = _baseValue;
        _modifiers.ForEach(x => finalValue += x);
        _moddedValue = finalValue;
    }

    public void ChangeBaseValue(int amount)
    {
        _baseValue += amount;
        CalculateModdedValue();

        if (_pcStatManager != null)
        {
            _pcStatManager.CalculateStatModifiers();
        }
        else
        {
            Debug.LogWarning($"No PCStatManager found on {_statType.ToString()}");
        }
//        OnBaseValueChanged?.Invoke();
    }

    public void AddModifier(int modifier)
    {
        _modifiers.Add(modifier);
        CalculateModdedValue();
    }

    public void ClearModifiers()
    {
        _modifiers.Clear();
        CalculateModdedValue();
    }
}