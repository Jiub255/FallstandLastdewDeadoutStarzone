using UnityEngine;

public class PCApproachEnemyState : PCState
{
    private Transform _target;
    private Collider _targetCollider;
    private float _weaponRangeSquared;
    private Transform _transform;

    public PCApproachEnemyState(PCStateMachine pcStateMachine, Transform target) : base(pcStateMachine)
    {
        _transform = pcStateMachine.PCDataSO.PCInstance.transform;

        _target = target;
        _targetCollider = target.GetComponent<Collider>();

        // TODO - Does this work? Seems a bit sloppy. 
        float weaponRange = _stateMachine.PCDataSO.Equipment.Weapon().AttackRange;
        _weaponRangeSquared = weaponRange * weaponRange;

        if (CharacterWithinRangeOfEnemy())
        {
            pcStateMachine.ChangeStateTo(pcStateMachine.Combat(target));
        }

        // Set destination. 
        pcStateMachine.PathNavigator.TravelPath(_target.position, _targetCollider);
    }

    public override void FixedUpdate(bool selected)
    {
        // Check if PC is within range of target (depends on the range of the weapon you're using). 
        if (CharacterWithinRangeOfEnemy())
        {
            _stateMachine.ChangeStateTo(_stateMachine.Combat(_target));
        }
    }

    private bool CharacterWithinRangeOfEnemy()
    {
        // TODO - What if target dies while you're running toward them? 
        if (_target != null)
        {
            if ((_target.position - _transform.position).sqrMagnitude < _weaponRangeSquared)
            {
                return true;
            }
        }
        return false;
    }

    public override void Update(bool selected) {}
    public override void Exit() {}
}