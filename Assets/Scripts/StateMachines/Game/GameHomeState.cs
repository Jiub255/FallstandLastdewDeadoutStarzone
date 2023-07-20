using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHomeState : GameState
{
    public GameHomeState(GameStateMachine gameStateMachine) : base(gameStateMachine)
    {
    }

    public override void SetActionMaps()
    {
        throw new System.NotImplementedException();
    }

    public override void SetTimeScale()
    {
        if (Time.timeScale != 0f) Time.timeScale = 1f;
    }
}