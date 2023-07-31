using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SOEnemiesData", fileName = "New Enemies Data SO")]
public class SOEnemiesData : ScriptableObject
{
	[SerializeField]
	private List<SOEnemyData> _enemyDataSOs;

	public List<SOEnemyData> EnemyDataSOs { get { return _enemyDataSOs; } }
}