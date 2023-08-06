using UnityEngine;

public class GameBuildState : GameState
{
    public GameBuildState(GameStateMachine gameStateMachine, InputManager inputManager) : base(gameStateMachine, inputManager) {}

    public override void SetTimeScale()
    {
        Time.timeScale = 0f;
    }
}