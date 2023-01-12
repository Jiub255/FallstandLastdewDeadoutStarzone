using System;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public static event Action<int, Transform> OnHitPC;

    [SerializeField]
    private int _damage = 1;

    private Animator _animator;
    private Collider _enemyCollider;

    [SerializeField]
    private LayerMask _playerCharacterLayer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        EnemyMovement.OnReachedPC += Attack;
    }

    private void OnDisable()
    {
        EnemyMovement.OnReachedPC -= Attack;
    }

    private void Attack(Transform enemyTransform, Transform playerTransform)
    {
        if (enemyTransform == transform)
        {
            // face PC
            transform.LookAt(playerTransform);
            // play attack animation
            _animator.SetTrigger("Attack");
            // boxcast immediately after animation to check if PC is still there
        }
    }

    // Animation event at the end of attack animation calls this method
    // TODO: Wont work with readonly animations from mixamo. Can't change them at all.
        // Maybe use a timer instead? Set to the animation's length? 
    public void DoDamage()
    {
        RaycastHit hit;
        // boxcast immediately after animation to check if PC is still there
        bool didBoxcastHit = Physics.BoxCast(_enemyCollider.bounds.center, transform.localScale, transform.forward, out hit, transform.rotation, 3f, _playerCharacterLayer);
        // if so, send damage signal to PC
        if (didBoxcastHit)
        {
            OnHitPC.Invoke(_damage, hit.transform);
        }
    }
}