using UnityEngine;

public class PCCombatState : PCState
{
    private Transform Target { get; }
    private float AttackDuration { get; }
    private int WeaponDamage { get; }
    private float Timer { get; set; }
    private Animator Animator { get; }
    private Transform Transform { get; }

    public PCCombatState(PCStateMachine pcStateMachine, InputManager inputManager, Transform target) : base(pcStateMachine, inputManager)
    {
        Target = target;

        AttackDuration = 1f / StateMachine.PCDataSO.Equipment.Weapon().AttackPerSecond;
        WeaponDamage = StateMachine.PCDataSO.WeaponDamage();

        Timer = 0f;
        Animator = pcStateMachine.Animator;
        Animator.SetBool("GunIdle", true);

        Transform = pcStateMachine.PCDataSO.PCInstance.transform;

        // Face the enemy. 
        Transform.LookAt(Target);

        // Stop character from moving. Not sure how to do it best though. 
//        characterController.NavMeshAgent.SetDestination(characterController.transform.position);
//        characterController.NavMeshAgent.isStopped = true;
//        characterController.NavMeshAgent.ResetPath();
        pcStateMachine.PathNavigator.StopMoving();
    }

    public override void Exit()
    {
        Animator.SetBool("GunIdle", false);
    }

    public override void Update(bool selected)
    {
        // TODO: Check if enemy is still in range first. 


        // Face the enemy. 
        Transform.LookAt(Target);

        Timer += Time.deltaTime;

        // Attack duration depends on your weapon and combat or speed stats. 
        if (Timer > AttackDuration)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Reset timer. 
        Timer = 0f;

        // Set attack animation trigger. 
        Animator.SetTrigger("Attack");

        // TODO - Use IDamageable interface here?
        // Just check that enemy is within range, and then if so hit connects. No need for overlapBox or any physics stuff. 
        Target.GetComponentInChildren<EnemyHealth>().GetHurt(WeaponDamage, this);
    }

    // How to check if enemy is dead? Dying enemy could send a signal with its instanceID.
    // Then players in combat state can check if it matches their current target, if so enemy is dead. 
    public void OnEnemyKilled()
    {
        Debug.Log("OnEnemyKilled called");

        StateMachine.ChangeStateTo(StateMachine.Idle());
    }

    public override void FixedUpdate(bool selected) {}
}