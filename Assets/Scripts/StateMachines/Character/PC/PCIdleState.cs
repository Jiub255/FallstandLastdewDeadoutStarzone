using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PCIdleState : PCState
{
    /// <summary>
    /// Called by right clicking while selected PC is in idle state. 
    /// </summary>
    public static event Action OnPCDeselected;

    private float _sightDistance;
    private Transform _transform;

    public PCIdleState(PCStateMachine pcStateMachine, SOPCMovementState pcMovementStateSO) : base(pcStateMachine)
    {
        _sightDistance = pcMovementStateSO.SightDistance;
        _transform = pcStateMachine.PCDataSO.PCInstance.transform;

        pcStateMachine.PathNavigator.StopMoving();
    }

    // Override to deselect instead of cancel action, only in idle state. 
    public override void CancelOrDeselect(InputAction.CallbackContext context)
    {
        _stateMachine.SetSelected(false);

        // Change selectedPCSO? Use an event? 
        // PCSelector listens, calls ChangePC(null). 
        OnPCDeselected?.Invoke();
    }

    public override void FixedUpdate(bool selected)
    {
        // Use OverlapSphere to check for enemies loot here? (if not selected)
        if (!selected)
        {
            Collider[] enemyCollidersInRange = Physics.OverlapSphere(_transform.position, _sightDistance, _stateMachine.PCDataSO.PCSharedDataSO.EnemyLayerMask);
            Collider[] lootCollidersInRange = Physics.OverlapSphere(_transform.position, _sightDistance, _stateMachine.PCDataSO.PCSharedDataSO.LootContainerLayerMask);

            if (enemyCollidersInRange.Length > 0)
            {
                // Get the closest enemy transform in range. 
                Transform enemyTransform = null;
                foreach (Collider collider in enemyCollidersInRange)
                {
                    // If this enemy is closer than closest so far, or transform is null because this is the first enemy checked, 
                    // (null check first to avoid null reference exception)
                    if (enemyTransform == null ||
                        (collider.transform.position - _transform.position).sqrMagnitude <
                        (enemyTransform.position - _transform.position).sqrMagnitude)
                    {
                        // Set this enemy transform as closest so far. 
                        enemyTransform = collider.transform;
                    }
                }

                // Change to ApproachEnemyState, with target as the closest enemy in range. 
                _stateMachine.ChangeStateTo(_stateMachine.ApproachEnemy(enemyTransform));
            }
            else if (lootCollidersInRange.Length > 0)
            {
                // Get the closest enemy transform in range. 
                Transform lootTransform = null;
                foreach (Collider collider in lootCollidersInRange)
                {
                    // If this loot is closer than closest so far, or transform is null because this is the first loot checked, 
                    // (null check first to avoid null reference exception)
                    if (lootTransform == null ||
                        (collider.transform.position - _transform.position).sqrMagnitude <
                        (lootTransform.position - _transform.position).sqrMagnitude)
                    {
                        // Set this loot transform as closest so far. 
                        lootTransform = collider.transform;
                    }
                }

                // Change to ApproachLootState, with target as the closest loot container in range. 
                _stateMachine.ChangeStateTo(_stateMachine.ApproachLoot(lootTransform.GetComponent<LootContainer>()));
            }
        }
    }

    public override void Update(bool selected) {}
}