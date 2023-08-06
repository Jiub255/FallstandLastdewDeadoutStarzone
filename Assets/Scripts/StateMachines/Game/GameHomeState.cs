using UnityEngine;

public class GameHomeState : GameState
{
    public GameHomeState(GameStateMachine gameStateMachine, InputManager inputManager) : base(gameStateMachine, inputManager) {}

    public override void SetTimeScale()
    {
        Time.timeScale = 1f;
    }
}