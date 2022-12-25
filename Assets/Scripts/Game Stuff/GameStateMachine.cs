using System;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public GameStateSO currentState;

    public event Action<GameStateSO> onChangedState;

    public bool ChangeStateAndActionMap(GameStateSO newState)
    {
        if (currentState.transferrableToStates.Contains(newState))
        {
            currentState = newState;
            // InputManager listens, changes action map corresponding to state
            onChangedState.Invoke(newState);
            return true;
        }
 
        return false;
    }
}