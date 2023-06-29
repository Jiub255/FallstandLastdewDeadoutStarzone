using UnityEngine;

public class PlayerCombatState : PlayerState
{
    private Transform _target;
    // public EnemyInjury _enemyInjury;
    private float _attackDuration;

    private float _timer;
    private Animator _animator;

    private Transform _transform;

    public PlayerCombatState(PlayerController characterController, Transform target, float attackDuration) : base(characterController)
    {
        _target = target;
        _attackDuration = attackDuration;

        _timer = 0f;
        _animator = _stateMachine.Animator;
        _animator.SetBool("GunIdle", true);

        _transform = _stateMachine.transform;

        // Get attack duration from current weapon/stats. 
        // Maybe keep these things in a SO for easy shared reference? But then each PC would need one.
        // Maybe just keep them on the PC instance? 
        // _attackDuration = ...;

        // Face the enemy. 
        _transform.LookAt(_target);
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
        // The attack should come from the weapon and stats. 
        // transform.parent.parent.GetComponentInChildren<Weapon>().CurrentWeapon.Attack(); or whatever. Use interface? 

        // Reset timer. 
        _timer = 0f;

        // Set attack animation trigger. 
        _animator.SetTrigger("Attack");

        // JUST FOR TESTING
        // TODO - Use IDamageable interface and do an OverlapBox in front of player. Same as in 3D-RPG. 
        _target.GetComponentInChildren<EnemyHealth>().GetHurt(25, this);
    }

    // How to check if enemy is dead? Dying enemy could send a signal with its instanceID.
    // Then players in combat state can check if it matches their current target, if so enemy is dead. 
    public void OnEnemyKilled()
    {
        // Set Target to null. 
        _target = null;

        _stateMachine.ChangeStateTo(_stateMachine.Idle());
    }

    public override void FixedUpdate() {}
}