using UnityEngine;

public class GameCombatMenusState : GameState
{
    public GameCombatMenusState(GameStateMachine gameStateMachine, InputManager inputManager) : base(gameStateMachine, inputManager) {}

    public override void SetTimeScale()
    {
        Time.timeScale = 0f;
    }
}