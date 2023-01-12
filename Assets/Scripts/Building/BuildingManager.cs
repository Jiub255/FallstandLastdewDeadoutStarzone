using System;
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
    private GameObject _currentBuildingPrefab;

    private GameObject _currentBuildingInstance;

    [SerializeField]
    private LayerMask _groundLayer;

    // For debug gizmos, so they dont draw in editor mode.
    private bool _started;

    [SerializeField]
    private float _rotationSpeed = 5f;

    //private InputActionMap buildActionMap;
    //private InputAction rotateAction;

    private void Start()
    {
        S.I.IM.PC.Build.PlaceBuilding.performed += PlaceBuilding;
        BuildingItemSO.OnSelectBuilding += ChangeCurrentBuilding;
        S.I.IM.PC.Build.CloseBuildMenu.performed += CloseBuildMenu;

        S.I.IM.PC.Build.RotateBuilding.started += RotateBuilding;

        //buildActionMap = S.I.IM.PC.Build;
        //rotateAction = S.I.IM.PC.Build.RotateBuilding;

        // Just for testing
        //MakeInstance();
        _started = true;
    }

    private void OnDisable()
    {
        S.I.IM.PC.Build.PlaceBuilding.performed -= PlaceBuilding;
        BuildingItemSO.OnSelectBuilding -= ChangeCurrentBuilding;
        S.I.IM.PC.Build.CloseBuildMenu.performed -= CloseBuildMenu;
      
        S.I.IM.PC.Build.RotateBuilding.performed -= RotateBuilding;
    }

    private void RotateBuilding(InputAction.CallbackContext obj)
    {
        if (_currentBuildingInstance != null)
        {
            _currentBuildingInstance.transform.Rotate(new Vector3(0f, Time.deltaTime * _rotationSpeed, 0f));
        }
    }

    private void Update()
    {
        if (_currentBuildingInstance != null)
        {
            // Move building to current mouse position on ground
            Ray ray = Camera.main.ScreenPointToRay(
                S.I.IM.PC.World.MousePosition.ReadValue<Vector2>());
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 1000, _groundLayer))
            {
                // Make sure to make buildings have their "pivot" on the bottom center, that is,
                // make an empty parent that holds the actual object and offset its y-coordinate so it's flush with the bottom of the parent.
                _currentBuildingInstance.transform.position = hitData.point;

                if (CanBuildHere())
                {
                    // Green highlight, allowed to build here
                    _currentBuildingInstance.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    _currentBuildingInstance.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    // Red highlight, can't build here
                    _currentBuildingInstance.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    _currentBuildingInstance.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }

    private void MakeInstance()
    {
        if (_currentBuildingPrefab != null)
        {
            if (_currentBuildingInstance != null)
            {
                Destroy(_currentBuildingInstance);
            } 

            _currentBuildingInstance = Instantiate(_currentBuildingPrefab);

            // put new building off camera until mouse is over ground. 100000 might be a bit excessive, not sure if it matters.
                // or put it in center of screen?
            _currentBuildingInstance.transform.position = Vector3.forward * 100000f;
        }
    }

    private void PlaceBuilding(InputAction.CallbackContext context)
    {
        if (CanBuildHere() && 
            _currentBuildingInstance != null)
        {
            // Turn off red/green highlights
            _currentBuildingInstance.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            _currentBuildingInstance.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

            // Stop controlling currentBuildingInstance with mouse, so essentially, build/set it.
            _currentBuildingInstance = null;

            // Make a new instance of same building to be the new currentBuildingInstance
            MakeInstance();
        }
    }

    // Gets called from a button in build menu, which calls event in BuildingItem.
    public void ChangeCurrentBuilding(GameObject newBuilding)
    {
        Debug.Log("Changing current building to " + newBuilding.name);

        _currentBuildingPrefab = newBuilding;

        MakeInstance();
    }

    private void DeselectCurrentBuilding()
    {
        if (_currentBuildingPrefab != null)
        {
            _currentBuildingPrefab = null;
        }
        if (_currentBuildingInstance != null)
        {
            Destroy(_currentBuildingInstance);
            _currentBuildingInstance = null;
        }
    }

    // Maybe set up rotate as two buttons instead? Then just subscribe their performed event to this method
/*    private void RotateBuilding(*//*InputAction.CallbackContext context*//*)
    {
        if (rotateAction.ReadValue<float>() > 0.5f)
        {
            // Rotate clockwise

        }
        else if (rotateAction.ReadValue<float>() < -0.5f)
        {
            // Rotate counter-clockwise

        }
    }*/

    private bool CanBuildHere()
    {
        BoxCollider boxCollider = _currentBuildingInstance.GetComponentInChildren<BoxCollider>();

        Collider[] colliders = Physics.OverlapBox(
            _currentBuildingInstance.transform.position + (Vector3.up * (boxCollider.bounds.size.y / 2)), 
            boxCollider.bounds.size / 2, 
            Quaternion.identity, 
            ~_groundLayer);

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

    private void CloseBuildMenu(InputAction.CallbackContext context)
    {
        if (_currentBuildingInstance != null)
        {
            Destroy(_currentBuildingInstance);
            _currentBuildingInstance = null;
        }

        if (_currentBuildingPrefab != null)
        {
            _currentBuildingPrefab = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (_currentBuildingInstance != null)
        {
            BoxCollider boxCollider = _currentBuildingInstance.GetComponentInChildren<BoxCollider>();

            Gizmos.color = Color.red;
            if (_started)
            {
                Gizmos.DrawWireCube(_currentBuildingInstance.transform.position + (Vector3.up * (boxCollider.bounds.size.y / 2)), boxCollider.bounds.size);
            }
        }
    }
}