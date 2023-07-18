using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Uses a "Building" prefab with the selected building icon child. Then attaches the actual building prefab as a child of it when selecting building.
public class BuildingManager : MonoBehaviour
{
    /*
    Want a free form building system, not grid based. 
    Have an empty "border" around buildings so you can't build them so close that you can't walk between them.
    Select thing to build from menu, then have it follow the mouse position on the ground
        Highlight it red if it intersects with something, green if it's able to be placed there
        Use a boxcast or something similar? Make sure its a little bigger than the building so people can walk around them.
        Rotate it using the mouse wheel
    Deactivate WorldGameplay action map when a building is selected, and activate BuildWorld action map
        WorldBuild has rotate building with mouse wheel, and right click deselects instead of dragging camera.
        Still have edge scrolling and keyboard camera movement and camera rotation by holding middle mouse button
        Change World action map to have the things common to World and BuildWorld, then have WorldGameplay and WorldBuild maps to cover the
            differences (mouse wheel, right click for now)
    */

    [SerializeField]
    private GameObject _buildingPrefab;
    private SOBuildingRecipe _currentBuildingRecipeSO;
    private GameObject _currentBuildingInstance;
    private SelectedBuildingIcon _selectedBuildingIcon;

    [SerializeField]
    private LayerMask _groundLayerMask;

    // For debug gizmos, so they dont draw in editor mode.
    //private bool _started;

    [SerializeField]
    private float _rotationSpeed = 50f;
    private Quaternion _rotation = Quaternion.identity;

    private bool _pointerOverUI = false;
    private EventSystem _eventSystem;

    [SerializeField, Tooltip("Snap angle to the next [Snap Angle]. Works best with a divisor of 360.")]
    private int _snapAngle = 45;

    private InputAction _mousePositionAction;

    [SerializeField]
    protected SOInventory _craftingInventorySO;


    private bool _angleSnapMode = false;
    [SerializeField, Tooltip("Toggle or hold modifier button (shift) to change angle snap mode")]
    private bool _toggle = false;

    private void Start()
    {
        _mousePositionAction = S.I.IM.PC.Camera.MousePosition;
        _eventSystem = EventSystem.current;

        // For debug gizmos, so they dont draw in editor mode.
       // _started = true;

        S.I.IM.PC.Build.RotateBuilding.started += RotateBuilding;
        S.I.IM.PC.Build.PlaceBuilding.started += PlaceBuilding;
        S.I.IM.PC.Build.SnapBuilding.started += SnapToNextAngle;
        S.I.IM.PC.NonCombatMenus.CloseBuildMenu.started += DeselectCurrentBuilding;

        S.I.IM.PC.Build.AngleSnapMode.started += ToggleAngleSnapMode;
        if (_toggle) S.I.IM.PC.Build.AngleSnapMode.canceled += ToggleAngleSnapMode;

        SOBuildingRecipe.OnSelectBuilding += SelectCurrentBuilding;
        InputManager.OnDeselectOrCancel += DeselectCurrentBuilding;
    }

    private void ToggleAngleSnapMode(InputAction.CallbackContext context)
    {
        _angleSnapMode = !_angleSnapMode;
    }

    // Use these two methods to switch between toggle and hold shift for changing angle snap mode. 
    // Going overboard, just practicing accessibility/customization stuff. 
    public void ToggleMode()
    {
        _toggle = true;
        S.I.IM.PC.Build.AngleSnapMode.canceled += ToggleAngleSnapMode;
    }
    public void HoldMode()
    {
        _toggle = false;
        S.I.IM.PC.Build.AngleSnapMode.canceled -= ToggleAngleSnapMode;
    }

    private void OnDisable()
    {
        S.I.IM.PC.Build.RotateBuilding.started -= RotateBuilding;
        S.I.IM.PC.Build.PlaceBuilding.started -= PlaceBuilding;
        S.I.IM.PC.Build.SnapBuilding.started -= SnapToNextAngle;
        S.I.IM.PC.NonCombatMenus.CloseBuildMenu.started -= DeselectCurrentBuilding;

        S.I.IM.PC.Build.AngleSnapMode.started -= ToggleAngleSnapMode;
        // TODO - How to check if event is subscribed? So you don't unnecessarily unsubscribe. 
        // Or could you just subscribe here real quick before unsubscribing to be sure? Seems hacky. 
        // Or, when you change _toggle, subscribe or unsubscribe there, so this _toggle check will be fine. 
        if (_toggle) S.I.IM.PC.Build.AngleSnapMode.canceled -= ToggleAngleSnapMode;

        SOBuildingRecipe.OnSelectBuilding -= SelectCurrentBuilding;
        InputManager.OnDeselectOrCancel -= DeselectCurrentBuilding;
    }

    private void FixedUpdate()
    {
        if (_currentBuildingInstance != null)
        {
            // Move building to current mouse position on ground
            Ray ray = Camera.main.ScreenPointToRay(_mousePositionAction.ReadValue<Vector2>());
            RaycastHit hitData;

            // TODO - Set maxDistance to zoom level plus a bit? Or is that just going overboard? 
            if (Physics.Raycast(ray, out hitData, 1000, _groundLayerMask))
            {
                // Make sure to make buildings have their "pivot" on the bottom center, that is,
                // make an empty parent that holds the actual object and offset its y-coordinate so it's flush with the bottom of the parent.
                _currentBuildingInstance.transform.position = hitData.point;

                SetHighlight();
            }
        }

        _pointerOverUI = _eventSystem.IsPointerOverGameObject();
    }

    private void RotateBuilding(InputAction.CallbackContext context)
    {
        if (_currentBuildingInstance != null)
        {
            _currentBuildingInstance.transform.Rotate(new Vector3(
                0f,
                Time.unscaledDeltaTime * _rotationSpeed * context.ReadValue<float>(), 
                0f));

            _rotation = _currentBuildingInstance.transform.rotation;

            SetHighlight();
        }
    }

    // TODO - Maybe have a "Snap to angles" mode toggle, and the rotate controls either rotate smoothly or in _snapAngle chunks depending. 
    // Or at least have the option to snap to previous angle too. 
    // Maybe holding/pressing shift can turn on/toggle angle snap mode. 
    private void SnapToNextAngle(InputAction.CallbackContext context)
    {
        if (_currentBuildingInstance != null)
        {
            for (int i = 0; i <= 360; i += _snapAngle)
            {
//                if (Mathf.Abs(_currentBuildingInstance.transform.rotation.eulerAngles.y - i) <= (_snapAngle / 2f))
                if (_rotation.eulerAngles.y < i)
                {
                    _currentBuildingInstance.transform.rotation = Quaternion.Euler(0f, i , 0f);
                    
                    _rotation = _currentBuildingInstance.transform.rotation;
                }
            }
        }
    }

    private void SetHighlight()
    {
        if (CanBuildHere())
        {
            // Green highlight, allowed to build here. 
            _selectedBuildingIcon.SetGreenMaterial();
        }
        else
        {
            // Red highlight, can't build here. 
            _selectedBuildingIcon.SetRedMaterial();
        }
    }

    private void MakeInstance()
    {
        if (_currentBuildingRecipeSO != null)
        {
            if (_currentBuildingInstance != null)
            {
                Destroy(_currentBuildingInstance);
                SetBuildingInstance(null);
            }

            SetBuildingInstance(Instantiate(_currentBuildingRecipeSO.BuildingPrefab));

            // Set building's rotation
            _currentBuildingInstance.transform.rotation = _rotation;
        }
    }

    // Use a "Building" prefab with the selected building icon child. Then attach the actual building prefab as a child of it in this script.
    private void SetBuildingInstance(GameObject buildingInstance)
    {
        if (buildingInstance != null)
        {
            GameObject buildingParent = Instantiate(_buildingPrefab);
            buildingInstance.transform.SetParent(buildingParent.transform);
            buildingInstance.transform.localPosition = Vector3.zero;
            _currentBuildingInstance = buildingParent;
            _selectedBuildingIcon = buildingInstance.GetComponent<SelectedBuildingIcon>();
//            _selectedBuildingIcon?.ActivateIcon();
        }
        else
        {
            _currentBuildingInstance = null;
            _selectedBuildingIcon = null;
        }
    }

    private void PlaceBuilding(InputAction.CallbackContext context)
    {
        // TODO: This might not work in builds, especially for android. Figure it out. 
        // Not sure what that means, look into it. Maybe the _pointerOverUI/eventSystem bit? 
        if (_currentBuildingInstance != null && !_pointerOverUI)
        {
            // Nesting if because CanBuildHere needs _currentBuildingInstance to be not null.
            if (CanBuildHere())
            {
                // TODO - Figure this out. 
                // Collider not working unless I switch it off and on again.
                // Could make it disabled in prefab then just enable it after placing?
                _currentBuildingInstance.GetComponentInChildren<Collider>().enabled = false;
                _currentBuildingInstance.GetComponentInChildren<Collider>().enabled = true;

                // Disconnect the actual building from the parent/selected icon. 
                _currentBuildingInstance.transform.GetComponentInChildren<Collider>().transform.parent = null;

                // Destroy the parent/selected icon. 
                Destroy(_currentBuildingInstance);

                // Setting instance to null stops the mouse from controlling its position, so it just stays where it was when you clicked (ie, it gets built).
                // This is only for when you place a building, the null check in MakeInstance is for when you select a building from the menu. 
                SetBuildingInstance(null);
//                _currentBuildingInstance = null;

                // Make a new instance of same building to be the new currentBuildingInstance
                MakeInstance();
            }
        }
    }

    // Gets called from a button in build menu, which calls event in BuildingItem.
    // Also called when selecting a building the first time, not just changing buildings.
    public void SelectCurrentBuilding(SOBuildingRecipe newBuildingRecipeSO)
    {
       // Debug.Log("Changing current building to " + newBuilding.name);

        _currentBuildingRecipeSO = newBuildingRecipeSO;

        MakeInstance();
    }

    private void DeselectCurrentBuilding(InputAction.CallbackContext context)
    {
        if (_currentBuildingRecipeSO != null)
        {
            _currentBuildingRecipeSO = null;
        }
        if (_currentBuildingInstance != null)
        {
            Destroy(_currentBuildingInstance);
            SetBuildingInstance(null);
//            _currentBuildingInstance = null;
        }
    }

    private bool CanBuildHere()
    {
        Collider[] collidersArray = Physics.OverlapBox(
            _selectedBuildingIcon.transform.position,
            _selectedBuildingIcon.transform.localScale,
//            Quaternion.identity, 
            _selectedBuildingIcon.transform.localRotation, 
            ~_groundLayerMask);

        // TODO - Do this better. 
        // Remove collisions with self.
        List<Collider> collidersList = collidersArray.ToList();
        for (int i = collidersList.Count - 1; i >= 0; i--)
        {
            //Debug.Log(_currentBuildingInstance.transform.GetChild(0).gameObject.GetInstanceID() + " collided with " + collider.gameObject.GetInstanceID());

            if (_selectedBuildingIcon.gameObject.GetInstanceID() == collidersList[i].gameObject.GetInstanceID())
            {
                collidersList.Remove(collidersList[i]);
            }
        }

        // TODO: Might need to fix this after adding collider to building prefabs.
        // Stupid hack anyway, do it better. 
        // collides with itself for now?, cheap hack fix
        if (collidersList.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

/*    private void OnDrawGizmos()
    {
        if (_currentBuildingInstance != null && _started)
        {
            Debug.Log(_currentBuildingInstance.transform.GetChild(0).transform.position + ", " + 
                _currentBuildingInstance.transform.GetChild(0).transform.localScale);

            //BoxCollider boxCollider = _currentBuildingInstance.GetComponentInChildren<BoxCollider>();
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_currentBuildingInstance.transform.GetChild(0).transform.position, _currentBuildingInstance.transform.GetChild(0).transform.localScale);
                *//*_currentBuildingInstance.transform.position + (Vector3.up * (boxCollider.bounds.size.y / 2)), boxCollider.bounds.size*//*
        }
    }*/
}