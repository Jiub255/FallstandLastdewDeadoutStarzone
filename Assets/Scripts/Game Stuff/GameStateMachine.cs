using System;
using UnityEngine;

// Send events whenever game state changes. One for which state you're leaving,
// one for which state you're entering.
public class GameStateMachine : MonoBehaviour
{
    public GameStateSO currentState;
    // Get this from onSceneChanged event?
    public SceneStateAllower currentSceneStateAllower;

/*    public event Action<GameStateSO> onLeftState;
    public event Action<GameStateSO> onEnteredState;*/
    // Passes previous state and new state
    public event Action<GameStateSO, GameStateSO> onChangedState;

    private void Awake()
    {
        currentSceneStateAllower = GameObject.Find(
            "Scene State Allower").GetComponent<SceneStateAllower>();
    }

    public bool ChangeStateAndActionMap(GameStateSO newState)
    {
        if (currentState.transferrableToStates.Contains(newState) && 
            currentSceneStateAllower.allowedGameStates.Contains(newState))
        {
            // InputManager listens, changes action maps corresponding to states
            /*onLeftState.Invoke(currentState);
            onEnteredState.Invoke(newState);*/
            onChangedState(currentState, newState);

            currentState = newState;
            
            return true;
        }
 
        return false;
    }

    public void ChangeSceneStateAllower()
    {

    }
}