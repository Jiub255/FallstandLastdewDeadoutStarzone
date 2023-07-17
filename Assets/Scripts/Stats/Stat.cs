using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public static event Action OnBaseValueChanged;

    public SOStatType StatTypeSO;
    [SerializeField]
    protected int _baseValue;

    protected List<int> _modifiers = new();

    // Only serialized to see in the editor. 
    // TODO - Keep this and just update it whenever stat/modifiers change, so you
    // aren't recalculating it every time you use it. 
    public int ModdedValue { get; private set; }

    public Stat(SOStatType statSO, int baseValue = 1)
    {
        StatTypeSO = statSO;

        _baseValue = baseValue;
        _modifiers = new();
    }

    public int GetValue()
    {
        int finalValue = _baseValue;
        _modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void ChangeBaseValue(int amount)
    {
        _baseValue += amount;
        ModdedValue = GetValue();
        // StatManager listens. 
        OnBaseValueChanged?.Invoke();
    }

    public void AddModifier(int modifier)
    {
        _modifiers.Add(modifier);

        ModdedValue = GetValue();
    }

    public void RemoveModifier(int modifier)
    {
        _modifiers.Remove(modifier);

        ModdedValue = GetValue();
    }

    public void ClearModifiers()
    {
        _modifiers.Clear();

        ModdedValue = GetValue();
    }
}