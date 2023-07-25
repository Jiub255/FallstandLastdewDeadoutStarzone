using UnityEngine;

public class EnemyCombatState : EnemyState
{
    private Transform _target;
    private float _attackRadiusSquared;
    private float _timeBetweenAttacks;

    private float _timer;
    private Transform _transform;

    // For changing animation eventually. Maybe use an animation controlling class instead?
//    private Animator _animator;

    public EnemyCombatState(EnemyStateMachine enemyStateMachine, Transform target, SOEnemyCombatState enemyCombatStateSO) : base(enemyStateMachine)
    {
        _transform = enemyStateMachine.transform;
        _target = target;
        _attackRadiusSquared = enemyCombatStateSO.AttackRadiusSquared;
        _timeBetweenAttacks = enemyCombatStateSO.TimeBetweenAttacks;
        _timer = 0f;

        // Stop character from moving. Not sure how to do it best though. 
        enemyStateMachine.PathNavigator.StopMoving();
    }

    public override void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _timeBetweenAttacks)
        {
            _timer = 0f;

            // Check if player is still within attack range. 
            if ((_target.position - _transform.position).sqrMagnitude > _attackRadiusSquared)
            {
                // Switch back to EnemyMoveToPCState. 
                // Just chooses another random target for now, but this should pass back target eventually. 
                _enemyStateMachine.ChangeStateTo(_enemyStateMachine.ApproachPC());
                return;
            }

            Attack();
        }
    }

    private void Attack()
    {
        // face PC
        _enemyStateMachine.transform.LookAt(_target);

        // play attack animation
        // _animator.SetTrigger("Attack");
        // boxcast immediately after animation to check if PC is still there

        // JUST FOR TESTING. Will use events and a better system eventually. 
        _target.GetComponentInChildren<PainInjuryManager>().GetHurt(10);
        Debug.Log("Got injured for 10 by " + _enemyStateMachine.transform.name);
    }

    public override void Exit() {}
    public override void FixedUpdate() {}
}