using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCSelectedState : PCBaseState
{
    public PCSelectedState(PCStateMachine currentContext, PCStateFactory pCStateFactory)
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