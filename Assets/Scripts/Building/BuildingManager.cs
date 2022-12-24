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
    private GameObject currentBuilding;

    [SerializeField]
    private LayerMask groundLayer;

/*    private PlayerInput playerInput;

    private InputAction mousePositionAction;*/

    private void Awake()
    {
/*        playerInput = GetComponent<PlayerInput>();
        mousePositionAction = playerInput.actions["MousePosition"];*/
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(
            MasterSingleton.Instance.InputManager.mousePositionAction.ReadValue<Vector2>());
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000, groundLayer))
        {
            // Make sure to make buildings have their "pivot" on the bottom center, that is,
            // make an empty parent that holds the actual object and offset its y-coordinate so it's flush with the bottom of the parent.
            currentBuilding.transform.position = hitData.point;

            if (CanBuildHere())
            {
                // Green highlight, allowed to build here
            }
            else
            {
                // Red highlight, can't build here
            }
        }
    }

    // Gets called from a button in build menu
    private void ChangeCurrentBuilding(GameObject newBuilding)
    {
        if (currentBuilding != null)
        {
            Destroy(currentBuilding);
        }

        currentBuilding = Instantiate(newBuilding);

        // put new building off camera until mouse is over ground. 100000 might be a bit excessive, not sure if it matters.
        currentBuilding.transform.position = Vector3.forward * 100000f;
    }

    private bool CanBuildHere()
    {
        BoxCollider boxCollider = currentBuilding.GetComponent<BoxCollider>();

        if (Physics.OverlapBox(
            currentBuilding.transform.position, boxCollider.bounds.size, Quaternion.identity) != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}