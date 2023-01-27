using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunState : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    [SerializeField]
    private GameObject _idleState; 

    private void OnEnable()
    {
        _navMeshAgent = transform.parent.parent.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // If PC is done moving, 
        if (!_navMeshAgent.pathPending &&
            _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance &&
            (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f))
        {
            // Activate Idle State. 
            _idleState.SetActive(true);

            // Deactivate this state. 
            gameObject.SetActive(false);
        }
    }
}