using UnityEngine;

public class GameHomeMenusState : GameState
{
    public GameHomeMenusState(GameStateMachine gameStateMachine) : base(gameStateMachine) {}

    public override void SetTimeScale()
    {
        Time.timeScale = 0f;
    }
}