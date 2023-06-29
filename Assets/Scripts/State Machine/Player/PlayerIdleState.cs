using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerState
{
    public static event Action OnPCDeselected;

    protected float _sightDistance;

    public PlayerIdleState(PlayerController characterController, float sightDistance) : base(characterController)
    {
        _sightDistance = sightDistance;

/*        // Not sure how to just clear the nav mesh agent. 
        _stateMachine.NavMeshAgent.isStopped = true;
        _stateMachine.NavMeshAgent.ResetPath();*/
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