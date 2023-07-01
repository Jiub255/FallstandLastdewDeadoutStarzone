using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyApproachPCState : State<EnemyController>
{
    private Transform _target;
    private float _attackRadius;

//    private PathNavigator _pathNavigator;
    private NavMeshAgent _navMeshAgent;
    private Transform _transform;

    public EnemyApproachPCState(EnemyController characterController, float attackRadius) : base(characterController)
    {
        _attackRadius = attackRadius;

//        _pathNavigator = characterController.PathNavigator;
        _navMeshAgent = characterController.NavMeshAgent;
        _transform = characterController.transform;
        // Set random PC as target (for now). 
        _target = ChooseRandomTarget();
        // Set target as NavMeshAgent destination
//        _pathNavigator.TravelPath(_target.position);
        _navMeshAgent.SetDestination(_target.position);
    }

    public override void Update()
    {
        if (Vector3.Distance(_target.position, _transform.position) <= _attackRadius)
        {
            // Switch to EnemyCombatState. 
            _stateMachine.ChangeStateTo(_stateMachine.Combat(_target));
        }
    }

    public override void FixedUpdate()
    {
        // Update destination in FixedUpdate so it's less costly. Probably unnecessary. 
//        _pathNavigator.TravelPath(_target.position);
        _navMeshAgent.SetDestination(_target.position);
        _transform.LookAt(_target);
    }

    private Transform ChooseRandomTarget()
    {
        // Choose random PC from all available as starting target (for now)
        List<GameObject> potentialTargets = new();
        potentialTargets.AddRange(GameObject.FindGameObjectsWithTag("PlayerCharacter"));
        int randomIndex = Random.Range(0, potentialTargets.Count);
        return potentialTargets[randomIndex].transform;
    }

    public override void Exit() {}
}