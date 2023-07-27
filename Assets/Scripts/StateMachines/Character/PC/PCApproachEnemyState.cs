using UnityEngine;

public class PCApproachEnemyState : PCState
{
    private Transform Target { get; }
    private Collider TargetCollider { get; }
    private float WeaponRangeSquared { get; }
    private Transform Transform { get; }

    public PCApproachEnemyState(PCStateMachine pcStateMachine, Transform target) : base(pcStateMachine)
    {
        Transform = pcStateMachine.PCDataSO.PCInstance.transform;

        Target = target;
        TargetCollider = target.GetComponent<Collider>();

        // TODO - Does this work? Seems a bit sloppy. 
        float weaponRange = StateMachine.PCDataSO.Equipment.Weapon().AttackRange;
        WeaponRangeSquared = weaponRange * weaponRange;

        if (CharacterWithinRangeOfEnemy())
        {
            pcStateMachine.ChangeStateTo(pcStateMachine.Combat(target));
        }

        // Set destination. 
        pcStateMachine.PathNavigator.TravelPath(Target.position, TargetCollider);
    }

    public override void FixedUpdate(bool selected)
    {
        // Check if PC is within range of target (depends on the range of the weapon you're using). 
        if (CharacterWithinRangeOfEnemy())
        {
            StateMachine.ChangeStateTo(StateMachine.Combat(Target));
        }
    }

    private bool CharacterWithinRangeOfEnemy()
    {
        // TODO - What if target dies while you're running toward them? 
        if (Target != null)
        {
            if ((Target.position - Transform.position).sqrMagnitude < WeaponRangeSquared)
            {
                return true;
            }
        }
        return false;
    }

    public override void Update(bool selected) {}
    public override void Exit() {}
}