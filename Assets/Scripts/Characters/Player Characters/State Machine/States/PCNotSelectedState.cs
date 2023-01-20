using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCNotSelectedState : PCBaseState
{
    public PCNotSelectedState(PCStateMachine currentContext, PCStateFactory pCStateFactory)
        : base(currentContext, pCStateFactory)
    {
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