using UnityEngine;

public class PCApproachLocationState : PCState
{
    private float _stoppingDistanceSquared;
    private Transform _transform;
    private Vector3 _destination;
         
    public PCApproachLocationState(PCStateMachine characterController, Vector3 destination) : base(characterController)
    {
        _destination = destination;

        // Not sure about this, might need to make it smaller/bigger. 
        _stoppingDistanceSquared = characterController.PathNavigator.StoppingDistance * characterController.PathNavigator.StoppingDistance * 1.2f;

        // Start traveling path. 
        characterController.PathNavigator.TravelPath(_destination, null);
    
        _transform = characterController.transform;
    }

    public override void Update()
    {
        // Check to see if within stopping distance. 
        if ((_transform.position - _destination).sqrMagnitude < _stoppingDistanceSquared)
        {
            _stateMachine.ChangeStateTo(_stateMachine.Idle());
        }
    }

    public override void FixedUpdate() {}
    public override void Exit() {}
}