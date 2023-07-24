using UnityEngine;

public class PCCombatState : PCState
{
    private Transform _target;
    private float _attackDuration;
    private int _attack;

    private float _timer; 
    private Animator _animator;
    private Transform _transform;

    public PCCombatState(PCStateMachine characterController, Transform target) : base(characterController)
    {
        _target = target;

        _attackDuration = 1f / _stateMachine.PCSO.Equipment.Weapon().AttackPerSecond;
        _attack = _stateMachine.PCSO.Attack();

        _timer = 0f;
        _animator = characterController.Animator;
        _animator.SetBool("GunIdle", true);

        _transform = characterController.transform;

        // Face the enemy. 
        _transform.LookAt(_target);

        // Stop character from moving. Not sure how to do it best though. 
//        characterController.NavMeshAgent.SetDestination(characterController.transform.position);
//        characterController.NavMeshAgent.isStopped = true;
//        characterController.NavMeshAgent.ResetPath();
        characterController.PathNavigator.StopMoving();
    }

    public override void Exit()
    {
        _animator.SetBool("GunIdle", false);
    }

    public override void Update()
    {
        // TODO: Check if enemy is still in range first. 


        // Face the enemy. 
        _transform.LookAt(_target);

        _timer += Time.deltaTime;

        // Attack duration depends on your weapon and combat or speed stats. 
        if (_timer > _attackDuration)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Reset timer. 
        _timer = 0f;

        // Set attack animation trigger. 
        _animator.SetTrigger("Attack");

        // TODO - Use IDamageable interface here?
        // Just check that enemy is within range, and then if so hit connects. No need for overlapBox or any physics stuff. 
        _target.GetComponentInChildren<EnemyHealth>().GetHurt(_attack, this);
    }

    // How to check if enemy is dead? Dying enemy could send a signal with its instanceID.
    // Then players in combat state can check if it matches their current target, if so enemy is dead. 
    public void OnEnemyKilled()
    {
        Debug.Log("OnEnemyKilled called");

        _stateMachine.ChangeStateTo(_stateMachine.Idle());
    }

    public override void FixedUpdate() {}
}