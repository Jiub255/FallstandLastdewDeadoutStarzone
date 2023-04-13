using UnityEngine;
using UnityEngine.AI;

public class ApproachLootState : MonoBehaviour
{
    // Gets set from mouse click event, or from non-selected PC detecting loot container while idle. 
    public Transform LootContainerTransform { get; set; }
    private LootContainer _lootContainer;
    private Vector3 _lootingPosition;

    [SerializeField]
    private float _lootDistance = 0.1f;

    [SerializeField]
    private GameObject _lootState;
    [SerializeField]
    private GameObject _idleState;

    private NavMeshAgent _agent;
   // private float _stoppingDistance;
    private Transform _transform;

    private void OnEnable()
    {
        _lootContainer = LootContainerTransform.GetComponentInChildren<LootContainer>();
        _lootingPosition = LootContainerTransform.GetChild(0).transform.position;
        _agent = transform.parent.parent.gameObject.GetComponent<NavMeshAgent>();
        _transform = _agent.transform;

        // Set new destination for PC's NavMeshAgent. 
        _agent.destination = _lootingPosition;
    }

    private void Update()
    {
        // What if container gets looted while you're on the way? 
        // Have an IsBeingLooted bool on LootContainer and check for it each frame here. 
        if (_lootContainer.IsBeingLooted || _lootContainer.Looted)
        {
            // Set state back to idle. 
            StateSwitcher.Switch(gameObject, _idleState);
        }
        else if (HaveReachedLoot())
        {
            // Unset NavMeshAgent destination? Can't set Vector3 to null. 
            _agent.isStopped = true;
            _agent.ResetPath();
            // Set stopping distance back to normal in case it got changed to 0f in HaveReachedLoot. 
            //_agent.stoppingDistance = _stoppingDistance;

            // Set LootContainerTransform in LootState. 
            _lootState.GetComponent<LootState>().LootContainerTransform = LootContainerTransform;

            // Switch state to LootState. 
            StateSwitcher.Switch(gameObject, _lootState);
        }
    }

    // TODO: Just let NavMeshAgent reach its destination naturally, since its heading to the looting 
    // position instead of the loot container's position like before. 
    // Check to see if it reached its destination in update instead of doing this check here. 
    private bool HaveReachedLoot()
    {
        Debug.Log($"NavMeshAgent.destination: {_agent.destination}, Looting Position: {_lootingPosition}");

        // NOT WORKING (the stopping distance stuff). 
        // Solves the problem of PC not moving towards loot if it was already close by temporarily setting stopping distance to zero.  
        // Stopping distance gets set back once it reaches the loot position. 
/*        if (Vector3.Distance(_transform.position, _lootingPosition) < _agent.stoppingDistance)
        {
            _stoppingDistance = _agent.stoppingDistance;

            _agent.stoppingDistance = 0f; 
        }*/

        return Vector3.Distance(_transform.position, _lootingPosition) < _lootDistance;
    }

/*    private bool HaveReachedDestination()
    {
        float distance = 0.0f;

        Vector3[] corners = _agent.path.corners;

        for (int c = 0; c < corners.Length - 1; ++c)
        {
            distance += Mathf.Abs((corners[c] - corners[c + 1]).magnitude);
        }

        return distance < _lootDistance;
    }*/
}