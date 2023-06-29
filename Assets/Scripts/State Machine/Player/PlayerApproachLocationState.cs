using UnityEngine;

public class PlayerApproachLocationState : PlayerState
{
    public PlayerApproachLocationState(PlayerController characterController, Vector3 destination) : base(characterController)
    {
        _stateMachine.NavMeshAgent.destination = destination;
    }

    public override void Exit()
    {
        // Clear/reset nav mesh agent? 
    }

    public override void Update()
    {
        // Check to see if within stopping distance? Or let nav mesh agent handle it? 
    }

    public override void FixedUpdate() {}
}