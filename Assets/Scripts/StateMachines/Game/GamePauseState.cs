using UnityEngine;

public class GamePauseState : GameState
{
    public GamePauseState(GameStateMachine gameStateMachine, InputManager inputManager) : base(gameStateMachine, inputManager) {}

    public override void SetTimeScale()
    {
        Time.timeScale = 0f;
    }
}