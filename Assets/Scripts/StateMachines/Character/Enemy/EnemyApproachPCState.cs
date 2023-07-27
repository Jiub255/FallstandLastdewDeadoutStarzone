using System.Collections.Generic;
using UnityEngine;

public class EnemyApproachPCState : EnemyState
{
    private Transform Target { get; }
    private Collider TargetCollider { get; }
    private float AttackRadiusSquared { get; }
    private PathNavigator PathNavigator { get; }
    private Transform Transform { get; }

    public EnemyApproachPCState(EnemyStateMachine enemyStateMachine, SOEnemyCombatState enemyCombatStateSO) : base(enemyStateMachine)
    {
        Transform = enemyStateMachine.transform;
        PathNavigator = enemyStateMachine.PathNavigator;
        AttackRadiusSquared = enemyCombatStateSO.AttackRadiusSquared;

        // Set random PC as target (for now). 
        Target = ChooseRandomTarget();
        TargetCollider = Target.GetComponent<Collider>();

        // Set target position as destination and start traveling path. 
        PathNavigator.TravelPath(Target.position, TargetCollider);
    }

    public override void Update()
    {
        if ((Target.position - Transform.position).sqrMagnitude <= AttackRadiusSquared)
        {
            // Switch to EnemyCombatState. 
            EnemyStateMachine.ChangeStateTo(EnemyStateMachine.Combat(Target));
        }
    }

    private Transform ChooseRandomTarget()
    {
        // Choose random PC from all available as starting target (for now)
        List<GameObject> potentialTargets = new();
        potentialTargets.AddRange(GameObject.FindGameObjectsWithTag("PlayerCharacter"));
        int randomIndex = Random.Range(0, potentialTargets.Count);
        return potentialTargets[randomIndex].transform;
    }

    public override void FixedUpdate() {}
    public override void Exit() {}
}