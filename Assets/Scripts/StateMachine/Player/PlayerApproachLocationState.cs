using UnityEngine;
using UnityEngine.AI;

public class PlayerApproachLocationState : PlayerState
{
    private float _stoppingDistanceSquared;
    private Transform _transform;
    private Vector3 _destination;
    private PathNavigator _pathNavigator;
//    private NavMeshAgent _navMeshAgent;
         
    public PlayerApproachLocationState(PlayerController characterController, Vector3 destination, float stoppingDistance) : base(characterController)
    {
//        _navMeshAgent = characterController.NavMeshAgent;
        _pathNavigator = characterController.PathNavigator;

        // Clear/reset nav mesh agent? 
        // Not sure how to just clear the nav mesh agent. 
//        _stateMachine.NavMeshAgent.isStopped = false;
//        _stateMachine.NavMeshAgent.ResetPath(); 

        _destination = destination;

        // Not sure about this, might need to make it smaller/bigger. 
//        _stoppingDistanceSquared = characterController.NavMeshAgent.stoppingDistance * characterController.NavMeshAgent.stoppingDistance * 1.2f;
//        _stoppingDistanceSquared = stoppingDistance * stoppingDistance;
        _stoppingDistanceSquared = characterController.PathNavigator.StoppingDistance * characterController.PathNavigator.StoppingDistance * 1.2f;

        // Start traveling path. 
//        _navMeshAgent.SetDestination(destination);
        characterController.PathNavigator.TravelPath(_destination, null);
    
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
        if ((_transform.position - _destination).sqrMagnitude < 0.2f/*_stoppingDistanceSquared*/)
        {
            _stateMachine.ChangeStateTo(_stateMachine.Idle());
        }
    }

    public override void FixedUpdate() 
    {
        // Recalculate path every fixed update in case something got in the way. 
//        _navMeshAgent.SetDestination(_destination);
//        _stateMachine.PathNavigator.TravelPath(_destination);
    }
}