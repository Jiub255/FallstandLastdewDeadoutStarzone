using UnityEngine.InputSystem;

public abstract class PlayerState : State<PlayerController>
{
    public PlayerState(PlayerController characterController) : base(characterController)
    {
        S.I.IM.PC.Home.Deselect.performed += CancelOrDeselect;
        S.I.IM.PC.Scavenge.Deselect.performed += CancelOrDeselect;
    }

    public override void Exit()
    {
        S.I.IM.PC.Home.Deselect.performed -= CancelOrDeselect;
        S.I.IM.PC.Scavenge.Deselect.performed -= CancelOrDeselect;
    }

    public virtual void CancelOrDeselect(InputAction.CallbackContext context)
    {
        // Cancel for most states, deselect only in idle state. 
        // So override this in idle state only. 
        _stateMachine.ChangeStateTo(_stateMachine.Idle());
    }
}