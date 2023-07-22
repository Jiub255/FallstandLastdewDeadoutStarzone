using UnityEngine;

public abstract class GameState
{
    protected GameStateMachine _gameStateMachine;

	public GameState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    public void SetActionMaps()
    {
        S.I.IM.EnableStateActionMaps(this);
    }

    public abstract void SetTimeScale();
}