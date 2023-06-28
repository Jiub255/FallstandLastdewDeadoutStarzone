using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCIdlingSubstate : PCBaseState
{
    public PCIdlingSubstate(PCStateMachine currentContext, PCStateFactory pCStateFactory)
: base(currentContext, pCStateFactory)
    {
    }

    public override void EnterState()
    {
        InitializeSubState();
    }

    public override void UpdateState()
    {
        // This method needs to be called last in UpdateState. 
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