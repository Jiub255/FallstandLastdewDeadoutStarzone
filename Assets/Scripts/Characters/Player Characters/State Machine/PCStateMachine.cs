using UnityEngine;

// Put all methods/variables/properties that the concrete states use here. 
// Put logic inside of concrete states (Can access machine and factory from states).
public class PCStateMachine : MonoBehaviour
{
    // State variables
	private PCBaseState _currentState;
    private PCStateFactory _states;

    // Properties
    public PCBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    private void Awake()
    {
        _states = new PCStateFactory(this);

        _currentState = _states.Idle();
        _currentState.EnterState();
    }

    private void Update()
    {
        _currentState.UpdateStates();
    }

    // Needed? Useless or harmful?
/*    private void OnDisable()
    {
        // Or ExitStates() if using it. 
        _currentState.ExitState();
    }*/
}