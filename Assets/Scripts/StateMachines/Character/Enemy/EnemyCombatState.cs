using UnityEngine;

public class EnemyCombatState : EnemyState
{
    private Transform Target { get; }
    private float AttackRadiusSquared { get; }
    private float TimeBetweenAttacks { get; }
    private float Timer { get; set; }
    private Transform Transform { get; }

    // For changing animation eventually. Maybe use an animation controlling class instead?
    //    private Animator _animator;

    public EnemyCombatState(EnemyStateMachine enemyStateMachine, Transform target, SOEnemyCombatState enemyCombatStateSO) : base(enemyStateMachine)
    {
        Transform = enemyStateMachine.transform;
        Target = target;
        AttackRadiusSquared = enemyCombatStateSO.AttackRadiusSquared;
        TimeBetweenAttacks = enemyCombatStateSO.TimeBetweenAttacks;
        Timer = 0f;

        // Stop character from moving. Not sure how to do it best though. 
        enemyStateMachine.PathNavigator.StopMoving();
    }

    public override void Update()
    {
        Timer += Time.deltaTime;

        if (Timer > TimeBetweenAttacks)
        {
            Timer = 0f;

            // Check if player is still within attack range. 
            if ((Target.position - Transform.position).sqrMagnitude > AttackRadiusSquared)
            {
                // Switch back to EnemyMoveToPCState. 
                // Just chooses another random target for now, but this should pass back target eventually. 
                EnemyStateMachine.ChangeStateTo(EnemyStateMachine.ApproachPC());
                return;
            }

            Attack();
        }
    }

    private void Attack()
    {
        // face PC
        EnemyStateMachine.transform.LookAt(Target);

        // play attack animation
        // _animator.SetTrigger("Attack");
        // boxcast immediately after animation to check if PC is still there

        // JUST FOR TESTING. Will use events and a better system eventually. 
        Target.GetComponentInChildren<PainInjuryManager>().GetHurt(10);
        Debug.Log("Got injured for 10 by " + EnemyStateMachine.transform.name);
    }

    public override void Exit() {}
    public override void FixedUpdate() {}
}