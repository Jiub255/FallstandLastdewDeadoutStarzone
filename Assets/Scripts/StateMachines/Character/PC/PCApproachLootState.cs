using UnityEngine;

// Gets set from mouse click event, or from non-selected PC detecting loot container while idle. 
public class PCApproachLootState : PCState
{
    private LootContainer LootContainer { get; }
    private Vector3 LootingPosition { get; }
    private float LootDistanceSquared { get; }
    private Transform Transform { get; }

    // Pass LootContainer in the constructor, and get NavMeshAgent and Transform from the PlayerController. 
    // Get lootingPosition and whatever else from LootContainer. 
    public PCApproachLootState(PCStateMachine pcStateMachine, LootContainer lootContainer) : base(pcStateMachine)
    {
        LootContainer = lootContainer;

        // Not sure about this, might need to make it smaller/bigger.
        LootDistanceSquared = pcStateMachine.PathNavigator.StoppingDistance * pcStateMachine.PathNavigator.StoppingDistance * 1.2f;

        LootingPosition = lootContainer.LootPositionTransform.position;
        Transform = pcStateMachine.PCDataSO.PCInstance.transform;

        // Set destination. 
        pcStateMachine.PathNavigator.TravelPath(LootingPosition, LootContainer.GetComponent<Collider>());
    }

    public override void Update(bool selected)
    {
        // What if container gets looted while you're on the way? Have an IsBeingLooted bool on LootContainer and check for it each frame here. 
        if (LootContainer.IsBeingLooted || LootContainer.Looted)
        {
            // Set state back to idle. 
            StateMachine.ChangeStateTo(StateMachine.Idle());
        }
        else if (HaveReachedLoot())
        {
            StateMachine.ChangeStateTo(StateMachine.Loot(LootContainer));
        }
    }

    // TODO: Just let NavMeshAgent reach its destination naturally, since its heading to the looting 
    // position instead of the loot container's position like before. 
    // Check to see if it reached its destination in update instead of doing this check here. 
    private bool HaveReachedLoot()
    {
        return (LootingPosition - Transform.position).sqrMagnitude < LootDistanceSquared;
    }

    public override void FixedUpdate(bool selected) {}
    public override void Exit() {}
}