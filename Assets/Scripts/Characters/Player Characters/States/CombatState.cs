using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : MonoBehaviour
{
    public Transform Target { get; set; }
   // public EnemyInjury _enemyInjury;

    private float _timer;
    private float _attackDuration;

    [SerializeField]
    private GameObject _idleState;

    private void OnEnable()
    {
        _timer = 0f;

        // Get attack duration from current weapon/stats. 
        // _attackDuration = ...;

        // Face the enemy. 
        transform.parent.parent.LookAt(Target);
    }

    private void Update()
    {
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
    }

    // How to check if enemy is dead? Dying enemy could send a signal with its instanceID.
    // Then players in combat state can check if it matches their current target, if so enemy is dead. 
}