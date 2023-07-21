using UnityEngine;

public class GamePauseState : GameState
{
    public GamePauseState(GameStateMachine gameStateMachine) : base(gameStateMachine) {}

    public override void SetTimeScale()
    {
        Time.timeScale = 0f;
    }
}