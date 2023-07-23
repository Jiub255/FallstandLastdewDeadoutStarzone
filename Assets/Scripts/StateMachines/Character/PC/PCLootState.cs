using System;
using UnityEngine;

public class PCLootState : PCState
{
    public static event Action<ItemAmount> OnLootItems;

    private LootContainer _lootContainer;

    private float _lootDuration; 
    
    private LootTimer _lootTimer;

    private float _timer;

    public PCLootState(PCController characterController, LootContainer lootContainer) : base(characterController)
    {
        _lootContainer = lootContainer;

        // (For now) Scavenging skill goes from 1 to 5, longest loot duration is 5 seconds, fastest is 1 second. So 6 - skill = duration. 
        _lootDuration = 6 - characterController.PCSO.Stats[StatType.Scavenging].ModdedValue;

        _lootTimer = characterController.gameObject.GetComponentInChildren<LootTimer>();

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

        // Stop character from moving. 
        characterController.PathNavigator.StopMoving();
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
            float percentOfTimeElapsed = (_lootDuration - _timer) / _lootDuration;
            _lootTimer.Tick(percentOfTimeElapsed);
        }
    }

    private void AddLoot()
    {
        foreach (ItemAmount itemAmount in _lootContainer.LootItemAmounts)
        {
            // Heard by PlayerInventoryManager, adds amount number of items. 
            OnLootItems?.Invoke(itemAmount);
        }
    }

    public override void FixedUpdate() {}
}