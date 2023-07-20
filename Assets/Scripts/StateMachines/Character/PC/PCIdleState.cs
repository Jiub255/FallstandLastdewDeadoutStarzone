using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PCIdleState : PCState
{
    public static event Action OnPCDeselected;

    protected float _sightDistance;

    public PCIdleState(PCStateMachine characterController, SOPCMovementState pcMovementStateSO) : base(characterController)
    {
        _sightDistance = pcMovementStateSO.SightDistance;

        // Stop character from moving. Not sure how to do it best though. 
//        characterController.NavMeshAgent.SetDestination(characterController.transform.position);
//        characterController.NavMeshAgent.isStopped = true;
//        characterController.NavMeshAgent.ResetPath();
        characterController.PathNavigator.StopMoving();
    }

    // Override to deselect instead of cancel action, only in idle state. 
    public override void CancelOrDeselect(InputAction.CallbackContext context)
    {
        _stateMachine.SetSelected(false);

        // Change selectedPCSO? Use an event? 
        // PCSelector listens, calls ChangePC(null). 
        OnPCDeselected?.Invoke();
    }

    public override void FixedUpdate()
    {
        // Use OverlapSphere to check for enemies loot here? (if not selected)
        if (!_stateMachine.Selected)
        {
            Collider[] enemyCollidersInRange = Physics.OverlapSphere(_stateMachine.transform.position, _sightDistance, _stateMachine.EnemyLayerMask);
            Collider[] lootCollidersInRange = Physics.OverlapSphere(_stateMachine.transform.position, _sightDistance, _stateMachine.LootContainerLayerMask);

            if (enemyCollidersInRange.Length > 0)
            {
                // ApproachEnemy state to enemyCollidersInRange[0]. 
                _stateMachine.ChangeStateTo(_stateMachine.ApproachEnemy(enemyCollidersInRange[0].transform));
            }
            else if (lootCollidersInRange.Length > 0)
            {
                // ApproachLoot state to lootCollidersInRange[0].
                _stateMachine.ChangeStateTo(_stateMachine.ApproachLoot(lootCollidersInRange[0].GetComponent<LootContainer>()));
            }
        }
    }

    public override void Update() {}
}