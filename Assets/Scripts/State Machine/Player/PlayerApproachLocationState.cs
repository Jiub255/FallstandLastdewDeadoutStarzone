using UnityEngine;

public class PlayerApproachLocationState : PlayerState
{
    private float _stoppingDistanceSquared;
    private Transform _transform;
    private Vector3 _destination;
//    private NavMeshAgent _navMeshAgent;
         
    public PlayerApproachLocationState(PlayerController characterController, Vector3 destination, float stoppingDistance) : base(characterController)
    {
        // Clear/reset nav mesh agent? 
        // Not sure how to just clear the nav mesh agent. 
//        _stateMachine.NavMeshAgent.isStopped = false;
//        _stateMachine.NavMeshAgent.ResetPath(); 

//        _navMeshAgent = characterController.NavMeshAgent;
//        _navMeshAgent.destination = destination;
//        _navMeshAgent.SetDestination(destination);
        _destination = destination;

//        _stoppingDistanceSquared = stoppingDistance * stoppingDistance;
        _stoppingDistanceSquared = characterController.PathNavigator.StoppingDistance * characterController.PathNavigator.StoppingDistance * 1.2f;

        // Start traveling path. 
        characterController.PathNavigator.TravelPath(_destination);
    
        _transform = characterController.transform;
    }

    public override void Exit()
    {
        // Clear/reset nav mesh agent? 
        // Not sure how to just clear the nav mesh agent. 
//        _stateMachine.NavMeshAgent.isStopped = true;
//        _stateMachine.NavMeshAgent.ResetPath();
    }

    public override void Update()
    {
        // Check to see if within stopping distance? Or let nav mesh agent handle it? 
/*        Debug.Log($"Squared distance: {(_transform.position - _stateMachine.NavMeshAgent.destination).sqrMagnitude}," +
            $"Position: {_transform.position}, NavMeshAgent destination: {_navMeshAgent.destination}, Stopping Distance Squared: {_stoppingDistanceSquared}");*/
        if ((_transform.position - _destination).sqrMagnitude < _stoppingDistanceSquared)
        {
            _stateMachine.ChangeStateTo(_stateMachine.Idle());
        }
    }

    public override void FixedUpdate() 
    {
        _stateMachine.PathNavigator.TravelPath(_destination);
    }
}