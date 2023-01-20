using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCWalkState : PCBaseState
{
    public PCWalkState(PCStateMachine currentContext, PCStateFactory pCStateFactory)
        : base(currentContext, pCStateFactory)
    {
        // Only do this in root state constructors.
        IsRootState = true;

        //InitializeSubState();
    }

    public override void EnterState()
    {
        InitializeSubState();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {

    }

    public override void CheckSwitchStates()
    {

    }
}