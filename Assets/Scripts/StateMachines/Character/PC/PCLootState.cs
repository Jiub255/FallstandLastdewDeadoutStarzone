using System;
using UnityEngine;

public class PCLootState : PCState
{
    public static event Action<ItemAmount> OnLootItems;

    private LootContainer _lootContainer;

    private float _lootDuration; 
    
    private LootTimer _lootTimer;

    private float _timer;

    public PCLootState(PCStateMachine pcStateMachine, LootContainer lootContainer) : base(pcStateMachine)
    {
        _lootContainer = lootContainer;

        // (For now) Scavenging skill goes from 1 to 5, longest loot duration is 5 seconds, fastest is 1 second. So 6 - skill = duration. 
        _lootDuration = 6 - pcStateMachine.PCDataSO.Stats[StatType.Scavenging].ModdedValue;

        _lootTimer = pcStateMachine.PCDataSO.PCInstance.GetComponentInChildren<LootTimer>();

        // Set LootContainer's IsBeingLooted to true. 
        _lootContainer.IsBeingLooted = true;

        // Move to exact position in front of loot. In Loot container game object, have a looting position child object to mark where to move. 
        pcStateMachine.PCDataSO.PCInstance.transform.position = _lootContainer.LootPositionTransform.position;

        // Face the loot container. 
        pcStateMachine.PCDataSO.PCInstance.transform.LookAt(_lootContainer.transform);

        // Set animation to looting. 
        pcStateMachine.Animator.SetTrigger("Loot");

        // Activate timer object. 
        _lootTimer.ActivateTimer(true);

        // Stop character from moving. 
        pcStateMachine.PathNavigator.StopMoving();
    }

    public override void Exit()
    {
        // Deactivate timer object
        _lootTimer.ActivateTimer(false);

        // Set LootContainer's IsBeingLooted to false. 
        _lootContainer.IsBeingLooted = false;
    }

    public override void Update(bool selected)
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

    public override void FixedUpdate(bool selected) {}
}