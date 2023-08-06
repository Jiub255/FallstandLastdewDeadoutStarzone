public abstract class GameState
{
    protected GameStateMachine _gameStateMachine;
    protected InputManager _inputManager;

	public GameState(GameStateMachine gameStateMachine, InputManager inputManager)
    {
        _gameStateMachine = gameStateMachine;
        _inputManager = inputManager;
    }

    public void SetActionMaps()
    {
        _inputManager.EnableStateActionMaps(this);
    }

    public abstract void SetTimeScale();
}