using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
	private SOEnemiesData _enemiesDataSO;

	public EnemyManager(SOEnemiesData enemiesDataSO)
    {
        _enemiesDataSO = enemiesDataSO;
    }

    public void UpdateStates()
    {
        foreach (SOEnemyData enemyDataSO in _enemiesDataSO.EnemyDataSOs)
        {
            enemyDataSO.ActiveState.Update();
        }
    }
    
    public void FixedUpdateStates()
    {
        foreach (SOEnemyData enemyDataSO in _enemiesDataSO.EnemyDataSOs)
        {
            enemyDataSO.ActiveState.FixedUpdate();
        }
    }
}