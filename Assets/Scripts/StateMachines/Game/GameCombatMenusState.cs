using UnityEngine;

public class GameCombatMenusState : GameState
{
    public GameCombatMenusState(GameStateMachine gameStateMachine) : base(gameStateMachine) {}

    public override void SetTimeScale()
    {
        Time.timeScale = 0f;
    }
}