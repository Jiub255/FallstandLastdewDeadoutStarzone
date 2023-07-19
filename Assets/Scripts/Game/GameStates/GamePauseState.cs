using UnityEngine;

public class GamePauseState : GameState
{
    public GamePauseState(GameStateMachine gameStateMachine) : base(gameStateMachine)
    {
        Debug.Log($"GamePauseState's this == {this}");
    }

    public override void SetActionMaps()
    {
        S.I.IM.EnableStateActionMaps(this);
    }

    public override void SetTimeScale()
    {
        if (Time.timeScale != 0f) Time.timeScale = 0f;
    }
}