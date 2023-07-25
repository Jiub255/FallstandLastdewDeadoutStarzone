using UnityEngine.InputSystem;

/// <summary>
/// TODO - Combine with CharacterState, rename whole thing PCState. 
/// </summary>
public abstract class PCState/* : CharacterState<PCStateMachine>*/
{
    protected PCStateMachine _stateMachine;


    public PCState(PCStateMachine characterController)/* : base(characterController)*/
    {
        InputManager.OnDeselectOrCancel += CancelOrDeselect;
    }

    public abstract void Update(bool selected = false);
    public abstract void FixedUpdate(bool selected = false);

    public virtual void Exit()
    {
        InputManager.OnDeselectOrCancel -= CancelOrDeselect;
    }

    public virtual void CancelOrDeselect(InputAction.CallbackContext context)
    {
        // Cancel for most states, deselect only in idle state. 
        // So override this in idle state only. 
        _stateMachine.ChangeStateTo(_stateMachine.Idle());
    }
}