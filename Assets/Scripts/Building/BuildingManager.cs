using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{
    // Want a free form building system, not grid based. 
        // Have an empty "border" around buildings so you can't build them so close that you can't walk between them.

    // Select thing to build from menu, then have it follow the mouse position on the ground
        // Highlight it red if it intersects with something, green if it's able to be placed there
            // Use a boxcast or something similar? Make sure its a little bigger than the building so people can walk around them.
        // Rotate it using the mouse wheel

    [SerializeField]
    private GameObject currentBuildingPrefab;

    private GameObject currentBuildingInstance;

    [SerializeField]
    private LayerMask groundLayer;

    // For debug gizmos, so they dont draw in editor mode.
    private bool started;

    private SceneStateAllower sceneStateAllower;

    private void Start()
    {
        S.I.IM.PC.World.Select.performed += PlaceBuilding;

        sceneStateAllower = GameObject.Find("Scene State Allower").GetComponent<SceneStateAllower>();

        // Just for testing
        //MakeInstance();
        started = true;
    }

    private void OnDisable()
    {
        S.I.IM.PC.World.Select.performed -= PlaceBuilding;
    }

    private void Update()
    {
        if (currentBuildingInstance != null)
        {
            // Move building to current mouse position on ground
            Ray ray = Camera.main.ScreenPointToRay(
                S.I.IM.PC.World.MousePosition.ReadValue<Vector2>());
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
                // or put it in center of screen?
            currentBuildingInstance.transform.position = Vector3.forward * 100000f;
        }
    }

    private void PlaceBuilding(InputAction.CallbackContext context)
    {
        if (CanBuildHere() && 
            currentBuildingInstance != null)
        {
            // Turn off red/green highlights
            currentBuildingInstance.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            currentBuildingInstance.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

            // Stop controlling currentBuildingInstance with mouse, so essentially, build/set it.
            currentBuildingInstance = null;

            // Make a new instance of same building to be the new currentBuildingInstance
            MakeInstance();
        }
    }

    // Gets called from a button in build menu
    private void ChangeCurrentBuilding(GameObject newBuilding)
    {
        currentBuildingPrefab = newBuilding;

        MakeInstance();
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