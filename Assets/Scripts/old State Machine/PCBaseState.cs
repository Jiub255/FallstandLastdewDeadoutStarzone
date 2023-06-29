using UnityEngine;

public abstract class PCBaseState
{
	// Fields 
	private bool _isRootState = false;
	private PCStateMachine _machine;
	private PCStateFactory _factory;
	private PCBaseState _currentSuperState;
	private PCBaseState _currentSubState;

	// Properties 
	protected bool IsRootState { set { _isRootState = value; } }
	protected PCStateMachine Machine { get { return _machine; } }
	protected PCStateFactory Factory { get { return _factory; } }
	public PCBaseState CurrentSuperState { get { return _currentSuperState; } }
	public PCBaseState CurrentSubState { get { return _currentSubState; } }

	// Constructor. Inherited constructors need to extend this. Why? What is happening exactly? 
	// It passes the concrete state 
	public PCBaseState(PCStateMachine currentContext, PCStateFactory pCStateFactory)
    {
		_machine = currentContext;
		_factory = pCStateFactory;
    }

	// Set animator values here, other state initialization stuff (bools, start timers, etc.). 
	public abstract void EnterState();

	// Put CheckSwitchStates() in here. Other things? 
	public abstract void UpdateState();

	// Set bools, reset timers, increment counts, etc. 
	public abstract void ExitState();

	// Put conditional logic for when/if to switch to each state here in each concrete state. 
	public abstract void CheckSwitchStates();

	// Initialize Substates from concrete states by using the context that is passed to it and if statements. 
	public abstract void InitializeSubState();

	// Update current state, substate, subsubstate, etc. 
	public void UpdateStates()
	{
		UpdateState();
		if (_currentSubState != null)
        {
			_currentSubState.UpdateStates();
        }
	}

	// Switch current (super or sub) state. 
	public void SwitchState(PCBaseState newState) 
	{
		// Current state exits state (Do ExitStates() instead if needed). 
		ExitState();

		// New state enters state. 
		newState.EnterState();

		if (_isRootState)
        {
			// Switch current state of context. 
			_machine.CurrentState = newState;
        }
		else if(_currentSuperState != null)
        {
			// Set the current superstate's substate to the new state. 
			_currentSuperState.SetSubState(newState);
        }
	}

	// Set new superstate. 
	protected void SetSuperState(PCBaseState newSuperState) 
	{
		_currentSuperState = newSuperState;
	}

	// Have to set new substate's superstate to this (current superstate) also. 
	protected void SetSubState(PCBaseState newSubState)
	{
		_currentSubState = newSubState;
		newSubState.SetSuperState(this);
	}

	// Might not need this. 
	// Exit current state, substate, subsubstate, etc. 
	/*	public void ExitStates()
		{
			ExitState();
			if (_currentSubState != null)
			{
				_currentSubState.ExitStates();
			}
		}*/
}