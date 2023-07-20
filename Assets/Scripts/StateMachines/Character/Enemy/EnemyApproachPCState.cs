using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyApproachPCState : CharacterState<EnemyController>
{
    private Transform _target;
    private Collider _targetCollider;
    private float _attackRadius;

    private PathNavigator _pathNavigator;
//    private NavMeshAgent _navMeshAgent;
    private Transform _transform;
    private Vector3 _lastPositionChecked;

    public EnemyApproachPCState(EnemyController characterController, float attackRadius) : base(characterController)
    {
        _attackRadius = attackRadius;


        _pathNavigator = characterController.PathNavigator;
//        _navMeshAgent = characterController.NavMeshAgent;
        _transform = characterController.transform;
        // Set random PC as target (for now). 
        _target = ChooseRandomTarget();

        _lastPositionChecked = _target.position;

        _targetCollider = _target.GetComponent<Collider>();

        Debug.Log($"{_stateMachine.transform.name}'s target: {(_target != null ? _target.name : "null")}");
        Debug.Log($"{_stateMachine.transform.name}'s target's position: {(_target != null ? _target.position : "none")}");
        Debug.Log($"{_stateMachine.transform.name}'s collider: {(_targetCollider != null ? _targetCollider.name : "null")}");
        Debug.Log($"{_stateMachine.transform.name}'s PathNavigator: {(_pathNavigator != null ? _pathNavigator.name : "null")}");

        // TODO - When starting scene with multiple enemies, some are getting null reference exception on this call even though the above debugs 
        // show that the references are not null. 
        // Set target position as destination and start traveling path. 
        _pathNavigator.TravelPath(_target.position, _targetCollider);
//        _navMeshAgent.SetDestination(_target.position);
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
/*        // Recalculate path every fixed update in case enemy moved. 
        // TODO - Only recalculate path if target has moved x units, or if target is y units (~ 1f) or closer to you?

        // If target has moved x or more units since last recalculation, 
        if (((_target.position - _lastPositionChecked).sqrMagnitude > 1f) ||
        // or target is within y units of you, 
        ((_target.position - _transform.position).sqrMagnitude < 1f))
        {
            _lastPositionChecked = _target.position;
            _pathNavigator.TravelPath(_target.position, _targetCollider);
            //            _navMeshAgent.SetDestination(_target.position);
        }

*//*        // Update destination in FixedUpdate in case PC is moving. 
        _pathNavigator.TravelPath(_target.position, _targetCollider);
//        _navMeshAgent.SetDestination(_target.position);
        _transform.LookAt(_target);*//**/
    }

    private Transform ChooseRandomTarget()
    {
        // Choose random PC from all available as starting target (for now)
        List<GameObject> potentialTargets = new();
        potentialTargets.AddRange(GameObject.FindGameObjectsWithTag("PlayerCharacter"));
        int randomIndex = Random.Range(0, potentialTargets.Count);
//        Debug.Log($"{_stateMachine.transform.name}'s potential targets: {potentialTargets.Count}, random index: {randomIndex}");
        return potentialTargets[randomIndex].transform;
    }

    public override void Exit() {}
}