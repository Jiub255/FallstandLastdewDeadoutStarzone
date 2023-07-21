using UnityEngine;

public class GameHomeState : GameState
{
    public GameHomeState(GameStateMachine gameStateMachine) : base(gameStateMachine) {}

    public override void SetTimeScale()
    {
        Time.timeScale = 1f;
    }
}