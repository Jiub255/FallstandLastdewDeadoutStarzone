using UnityEngine;
using UnityEngine.AI;

public class RunToLootState : MonoBehaviour
{
    // Gets set from mouse click event, or from non-selected PC detecting loot container while idle. 
    public Transform LootContainerTransform { get; set; }
    private LootContainer _lootContainer;
    private Vector3 _lootingPosition;

    [SerializeField]
    private float _lootDistance = 1.5f;

    [SerializeField]
    private GameObject _lootState;
    [SerializeField]
    private GameObject _idleState;

    private NavMeshAgent _agent;

    private void OnEnable()
    {
        _lootContainer = LootContainerTransform.GetComponentInChildren<LootContainer>();
        _lootingPosition = LootContainerTransform.GetChild(0).transform.position;
        _agent = transform.parent.parent.gameObject.GetComponent<NavMeshAgent>();

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

        if (HaveReachedLoot())
        {
            // Unset NavMeshAgent destination? Can't set Vector3 to null. 
            _agent.isStopped = true;
            _agent.ResetPath();

            // Set LootContainerTransform in LootState. 
            _lootState.GetComponent<LootState>().LootContainerTransform = LootContainerTransform;

            // Switch state to LootState. 
            StateSwitcher.Switch(gameObject, _lootState);
        }
    }

    private bool HaveReachedLoot()
    {
        if (Vector3.Distance(transform.position, _lootingPosition) < _lootDistance)
        {
            return true;
        }
        return false;
    }
}