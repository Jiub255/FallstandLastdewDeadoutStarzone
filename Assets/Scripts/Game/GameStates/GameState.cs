using UnityEngine;

public abstract class GameState
{
    protected GameStateMachine _gameStateMachine;

	public GameState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;

        Debug.Log($"GameState's this == {this}");
    }

    /// <summary>
    /// Set game state action maps through InputManager, set time scale, and reset PCInstance references if necessary. 
    /// </summary>
    /// Maybe reset instances on Exit instead? 
    public abstract void SetActionMaps();
    public abstract void SetTimeScale();
    public virtual void ResetPCInstanceReferencesOnSOs() {}
}