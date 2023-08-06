using UnityEngine;

public class GameCombatState : GameState
{
    public GameCombatState(GameStateMachine gameStateMachine, InputManager inputManager) : base(gameStateMachine, inputManager) {}

    public override void SetTimeScale()
    {
        Time.timeScale = 1f;
    }
}