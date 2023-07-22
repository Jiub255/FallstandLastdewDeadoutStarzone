using UnityEngine;

public class EnemyStateMachine : CharacterStateMachine<EnemyStateMachine>
{
    [SerializeField]
    private SOEnemyData _enemyDataSO;
/*    private float _attackRadius = 2f;
    [SerializeField]
    private float _timeBetweenAttacks = 2f;*/

    [Header("Any State Variables")]
    public PathNavigator PathNavigator;

    public EnemyCombatState Combat(Transform target) { return new EnemyCombatState(this, target, _enemyDataSO.EnemyCombatStateSO); }
    public EnemyApproachPCState ApproachPC() { return new EnemyApproachPCState(this, _enemyDataSO.EnemyCombatStateSO); }

    // Needs to be in Start and not Awake so the PCs have time to instantiate, then the enemy can choose a target PC
    // in its EnemyApproachPCState constructor. 
    private void /*Awake*/Start()
    {
        ChangeStateTo(ApproachPC());
    }
}