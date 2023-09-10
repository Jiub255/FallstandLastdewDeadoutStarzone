using UnityEngine;

public abstract class GameState
{
    protected InputManager InputManager { get; }
    protected float TimeScale { get; }

	public GameState(InputManager inputManager, float timeScale)
    {
        InputManager = inputManager;
        TimeScale = timeScale;
    }

    public void SetActionMaps()
    {
        InputManager.EnableStateActionMaps(this);
    }

    public void SetTimeScale()
    {
        Time.timeScale = TimeScale;
    }

    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();
}