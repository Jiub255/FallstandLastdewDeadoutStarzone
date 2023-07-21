using UnityEngine;

public abstract class GameState
{
    protected GameStateMachine _gameStateMachine;

	public GameState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    /// <summary>
    /// Set game state action maps through InputManager, set time scale, and reset PCInstance references if necessary. 
    /// </summary>
    /// Maybe reset instances on Exit instead? Or in SceneTransitionManager, game state doesn't affect it, scene change does. 
    public void SetActionMaps()
    {
        S.I.IM.EnableStateActionMaps(this);
    }

    public abstract void SetTimeScale();
}