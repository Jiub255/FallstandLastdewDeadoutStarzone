using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
	[SerializeField]
	private List<Stat> _statList = new();

	public List<Stat> StatList { get { return _statList; } }

	public Stat this[StatType statType]
    {
        get
        {
            foreach (Stat stat in StatList)
            {
                if (stat.StatType == statType)
                {
                    return stat;
                }
            }

            Debug.LogWarning($"No stat of type {statType} found in Statlist.");
            return null;
        }
    }
}