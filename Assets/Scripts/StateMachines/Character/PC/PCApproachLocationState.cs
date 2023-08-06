using UnityEngine;

public class PCApproachLocationState : PCState
{
    private float StoppingDistanceSquared { get; }
    private Transform Transform { get; }
    private Vector3 Destination { get; }
         
    public PCApproachLocationState(PCStateMachine pcStateMachine, InputManager inputManager, Vector3 destination) : base(pcStateMachine, inputManager)
    {
        Destination = destination;

        // Not sure about this, might need to make it smaller/bigger. 
        StoppingDistanceSquared = pcStateMachine.PathNavigator.StoppingDistance * pcStateMachine.PathNavigator.StoppingDistance * 1.2f;

        // Start traveling path. 
        pcStateMachine.PathNavigator.TravelPath(Destination, null);
    
        Transform = pcStateMachine.PCDataSO.PCInstance.transform;
    }

    public override void Update(bool selected)
    {
        // Check to see if within stopping distance. 
        if ((Transform.position - Destination).sqrMagnitude < StoppingDistanceSquared)
        {
            StateMachine.ChangeStateTo(StateMachine.Idle());
        }
    }

    public override void FixedUpdate(bool selected) {}
    public override void Exit() {}
}