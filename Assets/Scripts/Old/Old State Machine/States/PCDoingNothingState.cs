using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PCDoingNothingState : PCBaseState
{
    public PCDoingNothingState(PCStateMachine currentContext, PCStateFactory pCStateFactory)
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
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {
        // See which substate you should start in, based on environmental conditions. 
        NavMeshAgent navMeshAgent = Machine.gameObject.GetComponent<NavMeshAgent>();

        if (!navMeshAgent.pathPending && 
            navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && 
            (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f))
        {
            // Set idling substate. 
            SetSubState(Factory.GetIdlingSubstate());
        }
        else
        {
            // Set moving substate. 
            SetSubState(Factory.GetMovingSubstate());
        }

/*        if (Machine.gameObject.GetComponent<NavMeshAgent>().remainingDistance > 0.1f)
        {
            // Set moving substate. 
            SetSubState(Factory.GetMovingSubstate());
        }
        else
        {
            // Set idling substate. 
            SetSubState(Factory.GetIdlingSubstate());
        }*/
    }

    public override void CheckSwitchStates()
    {
        // If enemy is near, attack it (Fighting state). 

        // If enemy hits you, attack it (Fighting state). 

        // If loot is near (and no enemy is near), go loot (Looting state). 

    }
}