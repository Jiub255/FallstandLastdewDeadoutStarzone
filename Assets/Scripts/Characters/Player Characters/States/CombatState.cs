using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : MonoBehaviour
{
    public Transform Target { get; set; }
   // public EnemyInjury _enemyInjury;

    [SerializeField]
    private float _timer;
    // Only serialized for testing. Will get this value from weapon/stats eventually. 
    [SerializeField]
    private float _attackDuration = 1f;
    private Animator _animator;

    [SerializeField]
    private GameObject _idleState;

    private Transform _transform;

    private void OnEnable()
    {
        _timer = 0f;
        _animator = transform.parent.parent.GetComponentInChildren<Animator>();
        _animator.SetBool("GunIdle", true);

        _transform = transform.parent.parent;

        // Get attack duration from current weapon/stats. 
        // Maybe keep these things in a SO for easy shared reference? But then each PC would need one.
        // Maybe just keep them on the PC instance? 
        // _attackDuration = ...;

        // Face the enemy. 
        _transform.LookAt(Target);
    }

    private void OnDisable()
    {
        _animator.SetBool("GunIdle", false);
    }

    private void Update()
    {
        // TODO: Check if enemy is still in range first. 


        // Face the enemy. 
        _transform.LookAt(Target);

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
        Target.GetComponentInChildren<EnemyHealth>().GetHurt(25, this);
    }

    public void OnEnemyKilled()
    {
        // Set Target to null. 
        Target = null;

        StateSwitcher.Switch(gameObject, _idleState);
    }

    // How to check if enemy is dead? Dying enemy could send a signal with its instanceID.
    // Then players in combat state can check if it matches their current target, if so enemy is dead. 
}