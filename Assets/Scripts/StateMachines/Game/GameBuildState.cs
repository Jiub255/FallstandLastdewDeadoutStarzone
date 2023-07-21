using UnityEngine;

public class GameBuildState : GameState
{
    public GameBuildState(GameStateMachine gameStateMachine) : base(gameStateMachine) {}

    public override void SetTimeScale()
    {
        Time.timeScale = 0f;
    }
}