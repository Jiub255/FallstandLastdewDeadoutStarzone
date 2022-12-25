using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{
    // Want a free form building system, not grid based. 

    // Select thing to build from menu, then have it follow the mouse position on the ground
        // Highlight it red if it intersects with something, green if it's able to be placed there
            // Use a boxcast or something similar? Make sure its a little bigger than the building so people can walk around them.
        // Rotate it using the mouse wheel

    // How to handle build mode?
        // Treat it like a pause menu, except you can build/navigate build menu?
        // Use separate action map?
        // Use a game state machine?

    [SerializeField]
    private GameObject currentBuildingPrefab;

    private GameObject currentBuildingInstance;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private GameStateSO buildState;
    private bool inBuildMode = false;

    // For debug gizmos, so they dont draw in editor mode.
    private bool started;

    private SceneStateAllower sceneStateAllower;

    private void Start()
    {
        S.I.InputManager.selectAction.performed += PlaceBuilding;
        S.I.GameStateMachine.onChangedState += ToggleBuildMode;

        sceneStateAllower = GameObject.Find("Scene State Allower").GetComponent<SceneStateAllower>();

        MakeInstance();

        started = true;
    }

    private void OnDisable()
    {
        S.I.InputManager.selectAction.performed -= PlaceBuilding;
        S.I.GameStateMachine.onChangedState -= ToggleBuildMode;
    }

    private void Update()
    {
        if (currentBuildingInstance != null)
        {
            // Move building to current mouse position on ground
            Ray ray = Camera.main.ScreenPointToRay(
                S.I.InputManager.mousePositionAction.ReadValue<Vector2>());
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 1000, groundLayer))
            {
                // Make sure to make buildings have their "pivot" on the bottom center, that is,
                // make an empty parent that holds the actual object and offset its y-coordinate so it's flush with the bottom of the parent.
                currentBuildingInstance.transform.position = hitData.point;

                if (CanBuildHere())
                {
                    // Green highlight, allowed to build here
                    currentBuildingInstance.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    currentBuildingInstance.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    // Red highlight, can't build here
                    currentBuildingInstance.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    currentBuildingInstance.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }

    private void ToggleBuildMode(GameStateSO newState)
    {
        if (newState.name == buildState.name && sceneStateAllower.allowedGameStates.Contains(newState))
        {
            inBuildMode = true;
        }
        else
        {
            inBuildMode = false;
        }
    }

    private void MakeInstance()
    {
        if (currentBuildingPrefab != null)
        {
            if (currentBuildingInstance != null)
            {
                Destroy(currentBuildingInstance);
            } 

            currentBuildingInstance = Instantiate(currentBuildingPrefab);

            // put new building off camera until mouse is over ground. 100000 might be a bit excessive, not sure if it matters.
            currentBuildingInstance.transform.position = Vector3.forward * 100000f;
        }
    }

    private void PlaceBuilding(InputAction.CallbackContext context)
    {
        if (CanBuildHere())
        {
            currentBuildingInstance.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            currentBuildingInstance.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            currentBuildingInstance = null;
            MakeInstance();
        }
    }

    // Gets called from a button in build menu
    private void ChangeCurrentBuilding(GameObject newBuilding)
    {
        if (currentBuildingInstance != null)
        {
            Destroy(currentBuildingInstance);
        }

        currentBuildingPrefab = newBuilding;
        currentBuildingInstance = Instantiate(currentBuildingPrefab);

        // put new building off camera until mouse is over ground. 100000 might be a bit excessive, not sure if it matters.
        currentBuildingInstance.transform.position = Vector3.forward * 100000f;
    }

    private void DeselectCurrentBuilding()
    {
        if (currentBuildingPrefab != null)
        {
            currentBuildingPrefab = null;
        }
        if (currentBuildingInstance != null)
        {
            Destroy(currentBuildingInstance);
            currentBuildingInstance = null;
        }
    }

    private void RotateBuilding()
    {

    }

    private bool CanBuildHere()
    {
        BoxCollider boxCollider = currentBuildingInstance.GetComponentInChildren<BoxCollider>();

        Collider[] colliders = Physics.OverlapBox(
            currentBuildingInstance.transform.position + (Vector3.up * (boxCollider.bounds.size.y / 2)), 
            boxCollider.bounds.size / 2, 
            Quaternion.identity, 
            ~groundLayer);

        foreach (Collider collider in colliders)
        {
            Debug.Log(collider.gameObject.name);
        }

        // collides with itself for now, cheap hack fix
        if (colliders.Length == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        if (currentBuildingInstance != null)
        {
            BoxCollider boxCollider = currentBuildingInstance.GetComponentInChildren<BoxCollider>();

            Gizmos.color = Color.red;
            if (started)
            {
                Gizmos.DrawWireCube(currentBuildingInstance.transform.position + (Vector3.up * (boxCollider.bounds.size.y / 2)), boxCollider.bounds.size);
            }
        }
    }
}