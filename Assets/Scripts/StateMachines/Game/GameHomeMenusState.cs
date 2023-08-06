using UnityEngine;

public class GameHomeMenusState : GameState
{
    public GameHomeMenusState(GameStateMachine gameStateMachine, InputManager inputManager) : base(gameStateMachine, inputManager) {}

    public override void SetTimeScale()
    {
        Time.timeScale = 0f;
    }
}