using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCFightingState : PCBaseState
{
    public PCFightingState(PCStateMachine currentContext, PCStateFactory pCStateFactory)
    : base(currentContext, pCStateFactory)
    {
        // Only do this in root state constructors.
        IsRootState = true;
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
        // See which substate you should start in, based on environmental conditions. 

    }

    public override void CheckSwitchStates()
    {
        // If enemy dies, go to DoingNothing state. 

    }
}