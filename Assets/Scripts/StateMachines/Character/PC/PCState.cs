using UnityEngine.InputSystem;

/// <summary>
/// TODO - Combine with CharacterState, rename whole thing PCState. 
/// </summary>
public abstract class PCState/* : CharacterState<PCStateMachine>*/
{
    protected PCStateMachine StateMachine { get; }
    protected InputManager InputManager { get; }

    public PCState(PCStateMachine pcStateMachine, InputManager inputManager)/* : base(characterController)*/
    {
        StateMachine = pcStateMachine;
        inputManager.OnDeselectOrCancel += CancelOrDeselect;
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
        StateMachine.ChangeStateTo(StateMachine.Idle());
    }
}