using UnityEngine;

public class PlayerApproachLocationState : PlayerState
{
    private float _stoppingDistanceSquared;
    private Transform _transform;

    public PlayerApproachLocationState(PlayerController characterController, Vector3 destination, float stoppingDistance) : base(characterController)
    {
        _stateMachine.NavMeshAgent.destination = destination;
        _stoppingDistanceSquared = stoppingDistance * stoppingDistance;
    
        _transform = characterController.transform;
    }

    public override void Exit()
    {
        // Clear/reset nav mesh agent? 
        // Not sure how to just clear the nav mesh agent. 
        _stateMachine.NavMeshAgent.isStopped = true;
        _stateMachine.NavMeshAgent.ResetPath();
    }

    public override void Update()
    {
        // Check to see if within stopping distance? Or let nav mesh agent handle it? 
        if ((_transform.position - _stateMachine.NavMeshAgent.destination).sqrMagnitude < _stoppingDistanceSquared)
        {
            _stateMachine.ChangeStateTo(_stateMachine.Idle());
        }
    }

    public override void FixedUpdate() {}
}