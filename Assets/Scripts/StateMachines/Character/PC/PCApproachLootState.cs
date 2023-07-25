using UnityEngine;

// Gets set from mouse click event, or from non-selected PC detecting loot container while idle. 
public class PCApproachLootState : PCState
{
    private LootContainer _lootContainer;
    private Vector3 _lootingPosition;
    private float _lootDistanceSquared;
    private Transform _transform;

    // Pass LootContainer in the constructor, and get NavMeshAgent and Transform from the PlayerController. 
    // Get lootingPosition and whatever else from LootContainer. 
    public PCApproachLootState(PCStateMachine pcStateMachine, LootContainer lootContainer) : base(pcStateMachine)
    {
        _lootContainer = lootContainer;

        // Not sure about this, might need to make it smaller/bigger.
        _lootDistanceSquared = pcStateMachine.PathNavigator.StoppingDistance * pcStateMachine.PathNavigator.StoppingDistance * 1.2f;

        _lootingPosition = lootContainer.LootPositionTransform.position;
        _transform = pcStateMachine.PCDataSO.PCInstance.transform;

        // Set destination. 
        pcStateMachine.PathNavigator.TravelPath(_lootingPosition, _lootContainer.GetComponent<Collider>());
    }

    public override void Update(bool selected)
    {
        // What if container gets looted while you're on the way? Have an IsBeingLooted bool on LootContainer and check for it each frame here. 
        if (_lootContainer.IsBeingLooted || _lootContainer.Looted)
        {
            // Set state back to idle. 
            _stateMachine.ChangeStateTo(_stateMachine.Idle());
        }
        else if (HaveReachedLoot())
        {
            _stateMachine.ChangeStateTo(_stateMachine.Loot(_lootContainer));
        }
    }

    // TODO: Just let NavMeshAgent reach its destination naturally, since its heading to the looting 
    // position instead of the loot container's position like before. 
    // Check to see if it reached its destination in update instead of doing this check here. 
    private bool HaveReachedLoot()
    {
        return (_lootingPosition - _transform.position).sqrMagnitude < _lootDistanceSquared;
    }

    public override void FixedUpdate(bool selected) {}
    public override void Exit() {}
}