using UnityEngine;

// Gets set from mouse click event, or from non-selected PC detecting loot container while idle. 
public class PlayerApproachLootState : PlayerState
{
    private LootContainer _lootContainer;
    private Vector3 _lootingPosition;
    private float _lootDistanceSquared;
//    private NavMeshAgent _navMeshAgent;
    private Transform _transform;

    // Pass LootContainer in the constructor, and get NavMeshAgent and Transform from the PlayerController. 
    // Get lootingPosition and whatever else from LootContainer. 
    public PlayerApproachLootState(PlayerController characterController, LootContainer lootContainer, float lootDistance) : base(characterController)
    {
        _lootContainer = lootContainer;
//        _lootDistanceSquared = lootDistance * lootDistance;
        _lootDistanceSquared = characterController.PathNavigator.StoppingDistance * characterController.PathNavigator.StoppingDistance * 1.2f;

        _lootingPosition = lootContainer.LootPositionTransform.position;
//        _navMeshAgent = characterController.NavMeshAgent;
        _transform = characterController.transform;

        // Set destination. 
        characterController.PathNavigator.TravelPath(_lootingPosition);
//        _navMeshAgent.destination = lootContainer.LootPositionTransform.position;
//        _navMeshAgent.SetDestination(lootContainer.LootPositionTransform.position);
    }

    public override void Update()
    {
        // What if container gets looted while you're on the way? 
        // Have an IsBeingLooted bool on LootContainer and check for it each frame here. 
        if (_lootContainer.IsBeingLooted || _lootContainer.Looted)
        {
            // Set state back to idle. 
            _stateMachine.ChangeStateTo(_stateMachine.Idle());
        }
        else if (HaveReachedLoot())
        {
            // Set stopping distance back to normal in case it got changed to 0f in HaveReachedLoot. 
//            _agent.stoppingDistance = _stoppingDistance;

            _stateMachine.ChangeStateTo(_stateMachine.Loot(_lootContainer));
        }
    }

    // TODO: Just let NavMeshAgent reach its destination naturally, since its heading to the looting 
    // position instead of the loot container's position like before. 
    // Check to see if it reached its destination in update instead of doing this check here. 
    private bool HaveReachedLoot()
    {
        // Debug.Log($"NavMeshAgent.destination: {_agent.destination}, Looting Position: {_lootingPosition}");

        // NOT WORKING (the stopping distance stuff). 
        // Solves the problem of PC not moving towards loot if it was already close by temporarily setting stopping distance to zero.  
        // Stopping distance gets set back once it reaches the loot position. 
        /*        if (Vector3.Distance(_transform.position, _lootingPosition) < _agent.stoppingDistance)
                {
                    _stoppingDistance = _agent.stoppingDistance;

                    _agent.stoppingDistance = 0f; 
                }*/

/*        Debug.Log($"Squared distance: {(_lootingPosition*//*_navMeshAgent.destination*//* - _transform.position).sqrMagnitude}," +
            $"Looting position: {_lootingPosition}, Position: {_transform.position}, NavMeshAgent destination: {_navMeshAgent.destination}");*/
        return (_lootingPosition/*_navMeshAgent.destination*/ - _transform.position).sqrMagnitude < _lootDistanceSquared;
    }

    /*    private bool HaveReachedDestination()
        {
            float distance = 0.0f;

            Vector3[] corners = _agent.path.corners;

            for (int c = 0; c < corners.Length - 1; ++c)
            {
                distance += Mathf.Abs((corners[c] - corners[c + 1]).magnitude);
            }

            return distance < _lootDistance;
        }*/

    public override void Exit()
    {
        // Unset NavMeshAgent destination? Can't set Vector3 to null. 
/*        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath();*/
    }

    public override void FixedUpdate() {}
}