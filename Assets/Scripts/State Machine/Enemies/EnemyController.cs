using UnityEngine;
using UnityEngine.AI;

public class EnemyController : StateMachine<EnemyController>
{
    [SerializeField, Header("Combat State Variables")]
    private float _attackRadius = 2f;
    [SerializeField]
    private float _timeBetweenAttacks = 2f;

    [Header("Any State Variables")]
    public PathNavigator PathNavigator;
//    public NavMeshAgent NavMeshAgent;

    public EnemyCombatState Combat(Transform target) { return new EnemyCombatState(this, target, _attackRadius, _timeBetweenAttacks); }
    public EnemyApproachPCState ApproachPC() { return new EnemyApproachPCState(this, _attackRadius); }

    // Needs to be in Start and not Awake so the PCs have time to instantiate, then the enemy can choose a target PC
    // in its EnemyApproachPCState constructor. 
    private void /*Awake*/Start()
    {
        ChangeStateTo(ApproachPC());
    }
}