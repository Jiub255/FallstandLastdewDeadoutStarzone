using System.Collections.Generic;
using UnityEngine;

public class EnemyApproachPCState : EnemyState
{
    private Transform _target;
    private Collider _targetCollider;
    private float _attackRadiusSquared;
    private PathNavigator _pathNavigator;
    private Transform _transform;
//    private Vector3 _lastPositionChecked;

    public EnemyApproachPCState(EnemyStateMachine enemyStateMachine, SOEnemyCombatState enemyCombatStateSO) : base(enemyStateMachine)
    {
        _transform = enemyStateMachine.transform;
        _pathNavigator = enemyStateMachine.PathNavigator;
        _attackRadiusSquared = enemyCombatStateSO.AttackRadiusSquared;

        // Set random PC as target (for now). 
        _target = ChooseRandomTarget();
        _targetCollider = _target.GetComponent<Collider>();
//        _lastPositionChecked = _target.position;

        // Set target position as destination and start traveling path. 
        _pathNavigator.TravelPath(_target.position, _targetCollider);
    }

    public override void Update()
    {
        if ((_target.position - _transform.position).sqrMagnitude <= _attackRadiusSquared)
        {
            // Switch to EnemyCombatState. 
            _enemyStateMachine.ChangeStateTo(_enemyStateMachine.Combat(_target));
        }
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

    public override void FixedUpdate() {}
    public override void Exit() {}
}