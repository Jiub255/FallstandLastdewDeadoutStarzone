using UnityEngine;

public class PlayerLootState : PlayerState
{
    private LootContainer _lootContainer;
    private float _lootAnimationDuration;
    private InventorySO _inventorySO;
    private LootTimer _lootTimer;

    private float _timer;

    public PlayerLootState(PlayerController characterController, LootContainer lootContainer, AnimationClip lootAnimation, InventorySO inventorySO, LootTimer lootTimer) : base(characterController)
    {
        _lootContainer = lootContainer;
        _lootAnimationDuration = lootAnimation.length;
        _inventorySO = inventorySO;
        _lootTimer = lootTimer;

        // Set LootContainer's IsBeingLooted to true. 
        _lootContainer.IsBeingLooted = true;

        // Move to exact position in front of loot. In Loot container game object, have a looting position child object to mark where to move. 
        characterController.transform.position = _lootContainer.LootPositionTransform.position;

        // Face the loot container. 
        characterController.transform.LookAt(_lootContainer.transform);

        // Set animation to looting. 
        characterController.Animator.SetTrigger("Loot");

        // Activate timer object. 
        _lootTimer.ActivateTimer(true);

        // Stop character from moving. Not sure how to do it best though. 
//        characterController.NavMeshAgent.SetDestination(characterController.transform.position);
        characterController.NavMeshAgent.isStopped = true;
        characterController.NavMeshAgent.ResetPath();
//        characterController.PathNavigator.StopMoving();
    }

    public override void Exit()
    {
        // Deactivate timer object
        _lootTimer.ActivateTimer(false);

        // Set LootContainer's IsBeingLooted to false. 
        _lootContainer.IsBeingLooted = false;
    }

    public override void Update()
    {
        // Increment timer. 
        _timer -= Time.deltaTime;

        // If finished looting, 
        if (_timer <= 0)
        {
            // Add loot to inventory. 
            AddLoot();

            _lootContainer.Looted = true;

            _stateMachine.ChangeStateTo(_stateMachine.Idle());
        }
        else
        {
            float percentOfTimeElapsed = (_lootAnimationDuration - _timer) / _lootAnimationDuration;
            _lootTimer.Tick(percentOfTimeElapsed);
        }
    }

    private void AddLoot()
    {
        foreach (ItemAmount itemAmount in _lootContainer.LootItemAmounts)
        {
            if (itemAmount.ItemSO.GetType() == typeof(UsableItemSO))
            {
                // Check to see if you already have an itemAmount that matches the item, then add however many.
                if (_inventorySO.GetItemAmountFromItemSO(itemAmount.ItemSO) != null)
                {
                    _inventorySO.GetItemAmountFromItemSO(itemAmount.ItemSO).Amount += itemAmount.Amount;
                }
                else
                {
                    _inventorySO.ItemAmounts.Add(itemAmount);
                }
            }
        }
    }

    public override void FixedUpdate() {}
}