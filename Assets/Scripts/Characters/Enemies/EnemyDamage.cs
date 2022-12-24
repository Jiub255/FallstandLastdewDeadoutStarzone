using System;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    private Animator animator;
    private Collider enemyCollider;

    [SerializeField]
    private LayerMask playerCharacterLayer;

    public static event Action<int, Transform> onHitPC;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        EnemyMovement.onReachedPC += Attack;
    }

    private void OnDisable()
    {
        EnemyMovement.onReachedPC -= Attack;
    }

    private void Attack(Transform enemyTransform, Transform playerTransform)
    {
        if (enemyTransform == transform)
        {
            // face PC
            transform.LookAt(playerTransform);
            // play attack animation
            animator.SetTrigger("AttacK");
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
        bool didBoxcastHit = Physics.BoxCast(enemyCollider.bounds.center, transform.localScale, transform.forward, out hit, transform.rotation, 3f, playerCharacterLayer);
        // if so, send damage signal to PC
        if (didBoxcastHit)
        {
            onHitPC.Invoke(damage, hit.transform);
        }
    }
}