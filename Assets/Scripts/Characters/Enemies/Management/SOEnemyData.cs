using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/SOEnemyData", fileName = "New Enemy Data SO")]
public class SOEnemyData : ScriptableObject
{
	[SerializeField]
	private int _maxHealth;
	private int _currentHealth;
	[SerializeField]
	private List<ItemAmount> _loot;
	[SerializeField]
	private SOEnemyCombatState _enemyCombatStateSO;

	public int MaxHealth { get { return _maxHealth; } }
	public int CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
	public List<ItemAmount> Loot { get { return _loot; } }
	public SOEnemyCombatState EnemyCombatStateSO { get { return _enemyCombatStateSO; } }
}