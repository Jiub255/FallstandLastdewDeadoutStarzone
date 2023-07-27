using System;
using UnityEngine;

public class PCLootState : PCState
{
    public static event Action<ItemAmount> OnLootItems;

    private LootContainer LootContainer { get; }
    private float LootDuration { get; }
    private LootTimer LootTimer { get; }
    private float Timer { get; set; }

    public PCLootState(PCStateMachine pcStateMachine, LootContainer lootContainer) : base(pcStateMachine)
    {
        LootContainer = lootContainer;

        // (For now) Scavenging skill goes from 1 to 5, longest loot duration is 5 seconds, fastest is 1 second. So 6 - skill = duration. 
        LootDuration = 6 - pcStateMachine.PCDataSO.Stats[StatType.Scavenging].ModdedValue;

        LootTimer = pcStateMachine.PCDataSO.PCInstance.GetComponentInChildren<LootTimer>();

        // Set LootContainer's IsBeingLooted to true. 
        LootContainer.IsBeingLooted = true;

        // Move to exact position in front of loot. In Loot container game object, have a looting position child object to mark where to move. 
        pcStateMachine.PCDataSO.PCInstance.transform.position = LootContainer.LootPositionTransform.position;

        // Face the loot container. 
        pcStateMachine.PCDataSO.PCInstance.transform.LookAt(LootContainer.transform);

        // Set animation to looting. 
        pcStateMachine.Animator.SetTrigger("Loot");

        // Activate timer object. 
        LootTimer.ActivateTimer(true);

        // Stop character from moving. 
        pcStateMachine.PathNavigator.StopMoving();
    }

    public override void Exit()
    {
        // Deactivate timer object
        LootTimer.ActivateTimer(false);

        // Set LootContainer's IsBeingLooted to false. 
        LootContainer.IsBeingLooted = false;
    }

    public override void Update(bool selected)
    {
        // Increment timer. 
        Timer -= Time.deltaTime;

        // If finished looting, 
        if (Timer <= 0)
        {
            // Add loot to inventory. 
            AddLoot();

            LootContainer.Looted = true;

            StateMachine.ChangeStateTo(StateMachine.Idle());
        }
        else
        {
            float percentOfTimeElapsed = (LootDuration - Timer) / LootDuration;
            LootTimer.Tick(percentOfTimeElapsed);
        }
    }

    private void AddLoot()
    {
        foreach (ItemAmount itemAmount in LootContainer.LootItemAmounts)
        {
            // Heard by PlayerInventoryManager, adds amount number of items. 
            OnLootItems?.Invoke(itemAmount);
        }
    }

    public override void FixedUpdate(bool selected) {}
}