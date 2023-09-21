using UnityEngine;

/// <summary>
/// Using this struct instead of a (StatType, int) tuple so it can be shown in Unity inspector. <br/>
/// </summary>
[System.Serializable]
public struct StatValue
{
	[SerializeField]
	private StatType _statType;
	[SerializeField]
	private int _value;

	public StatType StatType { get { return _statType; } }
	public int Value { get { return _value; } }

	public StatValue(StatType statType, int value)
    {
		_statType = statType;
		_value = value;
    }
}