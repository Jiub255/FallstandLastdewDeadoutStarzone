using UnityEngine;

public class GameCombatState : GameState
{
    public GameCombatState(GameStateMachine gameStateMachine) : base(gameStateMachine) {}

    public override void SetTimeScale()
    {
        Time.timeScale = 1f;
    }
}