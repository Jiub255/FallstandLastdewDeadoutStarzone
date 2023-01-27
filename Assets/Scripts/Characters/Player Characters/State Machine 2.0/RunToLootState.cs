using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunToLootState : MonoBehaviour
{
    // Gets set from mouse click event, or from non-selected PC detecting loot container while idle. 
    public Transform LootContainerTransform { get; set; }
    private Vector3 _lootingPosition;

    [SerializeField]
    private float _lootDistance = 1.5f;

    [SerializeField]
    private GameObject _lootState;

    private void OnEnable()
    {
        _lootingPosition = LootContainerTransform.GetChild(0).transform.position;

        // Set new destination for PC's NavMeshAgent. 
        transform.parent.parent.gameObject.GetComponent<NavMeshAgent>().destination = _lootingPosition;
    }

    private void Update()
    {
        if (HaveReachedLoot())
        {
            // Move to exact position in front of loot. In Loot container game object, have a looting position child object to mark where to move. 
            transform.parent.parent.position = _lootingPosition;

            // Face the loot container
            transform.parent.parent.LookAt(LootContainerTransform);

            // Deactivate this state. 
            gameObject.SetActive(false);

            // Activate LootState. 
            _lootState.SetActive(true);

            // Activate selected substate if currently selected. 
            if (transform.GetChild(0).gameObject.activeSelf)
            {
                _lootState.transform.GetChild(0).gameObject.SetActive(true);
            }

            // Set LootContainerTransform in LootState. 
            _lootState.GetComponent<LootState>().LootContainerTransform = LootContainerTransform;
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