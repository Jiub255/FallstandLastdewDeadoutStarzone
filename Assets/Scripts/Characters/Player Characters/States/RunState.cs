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

    private void OnDisable()
    {
        _navMeshAgent.ResetPath();
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

            // Activate selected substate if currently selected. 
            if (transform.GetChild(0).gameObject.activeInHierarchy)
            {
                _idleState.transform.GetChild(0).gameObject.SetActive(true);
            }

            // Deactivate this state. 
            gameObject.SetActive(false);
        }
    }
}