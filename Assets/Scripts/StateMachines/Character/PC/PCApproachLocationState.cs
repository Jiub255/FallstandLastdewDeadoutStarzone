using UnityEngine;

public class PCApproachLocationState : PCState
{
    private float _stoppingDistanceSquared;
    private Transform _transform;
    private Vector3 _destination;
         
    public PCApproachLocationState(PCStateMachine pcStateMachine, Vector3 destination) : base(pcStateMachine)
    {
        _destination = destination;

        // Not sure about this, might need to make it smaller/bigger. 
        _stoppingDistanceSquared = pcStateMachine.PathNavigator.StoppingDistance * pcStateMachine.PathNavigator.StoppingDistance * 1.2f;

        // Start traveling path. 
        pcStateMachine.PathNavigator.TravelPath(_destination, null);
    
        _transform = pcStateMachine.PCDataSO.PCInstance.transform;
    }

    public override void Update(bool selected)
    {
        // Check to see if within stopping distance. 
        if ((_transform.position - _destination).sqrMagnitude < _stoppingDistanceSquared)
        {
            _stateMachine.ChangeStateTo(_stateMachine.Idle());
        }
    }

    public override void FixedUpdate(bool selected) {}
    public override void Exit() {}
}