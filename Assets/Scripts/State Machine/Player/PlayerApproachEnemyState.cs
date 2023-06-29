using UnityEngine;
using UnityEngine.AI;

public class PlayerApproachEnemyState : PlayerState
{
    private Transform _target;
    private float _weaponRangeSquared;

    private NavMeshAgent _navMeshAgent;
//    private Transform _pcTransform;

    public PlayerApproachEnemyState(PlayerController characterController, Transform target, float weaponRange) : base(characterController)
    {
        _target = target;
        _weaponRangeSquared = weaponRange * weaponRange;

        _navMeshAgent = _stateMachine.NavMeshAgent;
//        _pcTransform = _navMeshAgent.transform;

        // Set NavMeshAgent destination here. 
        _navMeshAgent.destination = _target.position;

        // Get weapon range from current weapon, and maybe stats affect it too. 
        // _weaponRange = _playerSomething.CurrentWeapon._range; 
    }

    public override void Exit()
    {
        // Send _target to CombatState here? 

        // Set destination to null or whatever. 

    }

    public override void Update()
    {
        // TODO - What if target dies while you're running toward them? 

        // Check if within range of target (depends on what weapon you're using). 
        if (CharacterWithinRangeOfEnemy())
        {
            // Unset NavMeshAgent destination? Can't set Vector3 to null. 
            _navMeshAgent.isStopped = true;
            _navMeshAgent.ResetPath();

            _stateMachine.ChangeStateTo(_stateMachine.Combat(_target));
        }
    }

    private bool CharacterWithinRangeOfEnemy()
    {
        // TODO - Check if Target becomes null. 
        if ((/*_pcTransform*/_target.position - _navMeshAgent.transform.position).sqrMagnitude < _weaponRangeSquared)
        {
            return true;
        }
        return false;
    }

    public override void FixedUpdate() {}
}