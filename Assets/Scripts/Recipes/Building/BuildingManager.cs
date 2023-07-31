using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// Uses a "Building" prefab with the selected building icon child. Then attaches the actual building prefab as a child of it when selecting building.
public class BuildingManager
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

    /*    [SerializeField]
        private GameObject _buildingPrefab;
        [SerializeField]
        private LayerMask _groundLayerMask;
        [SerializeField]
        private float _rotationSpeed = 50f;
        [SerializeField, Tooltip("Snap angle to the next [Snap Angle]. Works best with a divisor of 360.")]
        private int _snapAngle = 45;*/

    private SOBuildingData BuildingDataSO { get; }

    // Keep here. 
    private InputAction MousePositionAction { get; }
    private InputManager InputManager { get; }


    // For debug gizmos, so they dont draw in editor mode.
    //private bool _started;

    // Just testing stuff out here. Not gonna keep it. 
/*    private bool _angleSnapMode = false;
    [SerializeField, Tooltip("Toggle or hold modifier button (shift) to change angle snap mode")]
    private bool _toggle = false;*/

    public BuildingManager(SOBuildingData buildingDataSO)
    {
        BuildingDataSO = buildingDataSO;

        MousePositionAction = S.I.IM.PC.Camera.MousePosition;
        InputManager = S.I.IM;

        // For debug gizmos, so they dont draw in editor mode.
       // _started = true;

        S.I.IM.PC.Build.RotateBuilding.started += RotateBuilding;
        S.I.IM.PC.Build.PlaceBuilding.started += PlaceBuilding;
        S.I.IM.PC.Build.SnapBuilding.started += SnapToNextAngle;
        S.I.IM.PC.NonCombatMenus.CloseBuildMenu.started += DeselectCurrentBuilding;

        SOBuilding.OnSelectBuilding += SelectCurrentBuilding;
        InputManager.OnDeselectOrCancel += DeselectCurrentBuilding;

/*        S.I.IM.PC.Build.AngleSnapMode.started += ToggleAngleSnapMode;
        if (_toggle) S.I.IM.PC.Build.AngleSnapMode.canceled += ToggleAngleSnapMode;*/
    }

/*    private void ToggleAngleSnapMode(InputAction.CallbackContext context)
    {
        _angleSnapMode = !_angleSnapMode;
    }

    // Use these two methods to switch between toggling and holding shift to change angle snap mode. 
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
    }*/

    public void OnDisable()
    {
        S.I.IM.PC.Build.RotateBuilding.started -= RotateBuilding;
        S.I.IM.PC.Build.PlaceBuilding.started -= PlaceBuilding;
        S.I.IM.PC.Build.SnapBuilding.started -= SnapToNextAngle;
        S.I.IM.PC.NonCombatMenus.CloseBuildMenu.started -= DeselectCurrentBuilding;

        SOBuilding.OnSelectBuilding -= SelectCurrentBuilding;
        InputManager.OnDeselectOrCancel -= DeselectCurrentBuilding;

/*        S.I.IM.PC.Build.AngleSnapMode.started -= ToggleAngleSnapMode;
        if (_toggle) S.I.IM.PC.Build.AngleSnapMode.canceled -= ToggleAngleSnapMode;*/
    }

    public void FixedUpdate()
    {
        if (BuildingDataSO.CurrentBuildingInstance != null)
        {
            // Move building to current mouse position on ground
            Ray ray = Camera.main.ScreenPointToRay(MousePositionAction.ReadValue<Vector2>());
            RaycastHit hitData;

            // TODO - Set maxDistance to zoom level plus a bit? Or is that just going overboard? 
            if (Physics.Raycast(ray, out hitData, 1000, BuildingDataSO.GroundLayerMask))
            {
                // Make sure to make buildings have their "pivot" on the bottom center, that is,
                // make an empty parent that holds the actual object and offset its y-coordinate so it's flush with the bottom of the parent.
                BuildingDataSO.CurrentBuildingInstance.transform.position = hitData.point;

                SetHighlight();
            }
        }
    }

    private void RotateBuilding(InputAction.CallbackContext context)
    {
        if (BuildingDataSO.CurrentBuildingInstance != null)
        {
            BuildingDataSO.CurrentBuildingInstance.transform.Rotate(new Vector3(
                0f,
                Time.unscaledDeltaTime * BuildingDataSO.RotationSpeed * context.ReadValue<float>(), 
                0f));

            BuildingDataSO.Rotation = BuildingDataSO.CurrentBuildingInstance.transform.rotation;

            SetHighlight();
        }
    }

    // TODO - Maybe have a "Snap to angles" mode toggle, and the rotate controls either rotate smoothly or in _snapAngle chunks depending. 
    // Or at least have the option to snap to previous angle too. 
    // Maybe holding/pressing shift can turn on/toggle angle snap mode. 
    private void SnapToNextAngle(InputAction.CallbackContext context)
    {
        if (BuildingDataSO.CurrentBuildingInstance != null)
        {
            for (int i = 0; i <= 360; i += BuildingDataSO.SnapAngle)
            {
//                if (Mathf.Abs(_currentBuildingInstance.transform.rotation.eulerAngles.y - i) <= (_snapAngle / 2f))
                if (BuildingDataSO.Rotation.eulerAngles.y < i)
                {
                    BuildingDataSO.CurrentBuildingInstance.transform.rotation = Quaternion.Euler(0f, i , 0f);
                    
                    BuildingDataSO.Rotation = BuildingDataSO.CurrentBuildingInstance.transform.rotation;
                }
            }
        }
    }

    private void SetHighlight()
    {
        if (CanBuildHere())
        {
            // Green highlight, allowed to build here. 
            BuildingDataSO.SelectedBuildingIcon.SetGreenMaterial();
        }
        else
        {
            // Red highlight, can't build here. 
            BuildingDataSO.SelectedBuildingIcon.SetRedMaterial();
        }
    }

    private void MakeInstance()
    {
        if (BuildingDataSO.CurrentBuildingRecipeSO != null)
        {
            if (BuildingDataSO.CurrentBuildingInstance != null)
            {
                Object.Destroy(BuildingDataSO.CurrentBuildingInstance);
                SetBuildingInstance(null);
            }

            SetBuildingInstance(Object.Instantiate(BuildingDataSO.CurrentBuildingRecipeSO.BuildingPrefab));

            // Set building's rotation
            BuildingDataSO.CurrentBuildingInstance.transform.rotation = BuildingDataSO.Rotation;
        }
    }

    // Use a "Building" prefab with the selected building icon child. Then attach the actual building prefab as a child of it in this script.
    private void SetBuildingInstance(GameObject buildingInstance)
    {
        if (buildingInstance != null)
        {
            GameObject buildingParent = Object.Instantiate(BuildingDataSO.BuildingPrefab);
            buildingInstance.transform.SetParent(buildingParent.transform);
            buildingInstance.transform.localPosition = Vector3.zero;
            BuildingDataSO.CurrentBuildingInstance = buildingParent;
            BuildingDataSO.SelectedBuildingIcon = buildingInstance.GetComponent<SelectedBuildingIcon>();
//            _selectedBuildingIcon?.ActivateIcon();
        }
        else
        {
            BuildingDataSO.CurrentBuildingInstance = null;
            BuildingDataSO.SelectedBuildingIcon = null;
        }
    }

    /// <summary>
    /// Filters out all of the SORecipes that you don't have the required crafting buildings for. 
    /// </summary>
    public List<T> GetHaveRequiredBuildingsRecipes<T>(List<T> haveEnoughItemsRecipes) where T : SORecipe
    {
        // Does this fancy LINQ work? 
        return haveEnoughItemsRecipes.Where(recipeSO => 
            recipeSO.RequiredBuildings.Where(craftingBuildingSO => 
            !BuildingDataSO.Buildings.Contains(craftingBuildingSO)).ToList().Count == 0).ToList();

/*        List<T> haveRequiredBuildingsRecipes = new();

        foreach (T recipe in haveEnoughItemsRecipes)
        {
            foreach (SOBuilding building in recipe.RequiredBuildings)
            {
                if (!BuildingDataSO.Buildings.Contains(building))
                {
                    break;
                }
            }

            // Can only reach this point if you have built all required buildings. 
            haveRequiredBuildingsRecipes.Add(recipe);
        }

        return haveRequiredBuildingsRecipes;*/
    }

    private void PlaceBuilding(InputAction.CallbackContext context)
    {
        // TODO: This might not work in builds, especially for android. Figure it out. 
        // Not sure what that means, look into it. Maybe the _pointerOverUI/eventSystem bit? 
        if (BuildingDataSO.CurrentBuildingInstance != null && !InputManager.PointerOverUI)
        {
            // Nesting if because CanBuildHere needs _currentBuildingInstance to be not null.
            if (CanBuildHere())
            {
                // Add SOBuilding to Buildings list. 
                BuildingDataSO.Buildings.Add(BuildingDataSO.CurrentBuildingRecipeSO);

                // TODO - Figure this out. 
                // Collider not working unless I switch it off and on again.
                // Could make it disabled in prefab then just enable it after placing?
                BuildingDataSO.CurrentBuildingInstance.GetComponentInChildren<Collider>().enabled = false;
                BuildingDataSO.CurrentBuildingInstance.GetComponentInChildren<Collider>().enabled = true;

                // Disconnect the actual building from the parent/selected icon. 
                BuildingDataSO.CurrentBuildingInstance.transform.GetComponentInChildren<Collider>().transform.parent = null;

                // Destroy the parent/selected icon. 
                Object.Destroy(BuildingDataSO.CurrentBuildingInstance);

                // Setting instance to null stops the mouse from controlling its position, so it just stays where it was when you clicked (ie, it gets built) 
                // (this is only for when you place a building, the null check in MakeInstance is for when you select a building from the menu). 
                SetBuildingInstance(null);
//                _currentBuildingInstance = null;

                // Make a new instance of same building to be the new currentBuildingInstance
                MakeInstance();
            }
        }
    }

    // Gets called from a button in build menu, which calls event in BuildingItem.
    // Also called when selecting a building the first time, not just changing buildings.
    public void SelectCurrentBuilding(SOBuilding newBuildingRecipeSO)
    {
       // Debug.Log("Changing current building to " + newBuilding.name);

        BuildingDataSO.CurrentBuildingRecipeSO = newBuildingRecipeSO;

        MakeInstance();
    }

    private void DeselectCurrentBuilding(InputAction.CallbackContext context)
    {
        if (BuildingDataSO.CurrentBuildingRecipeSO != null)
        {
            BuildingDataSO.CurrentBuildingRecipeSO = null;
        }
        if (BuildingDataSO.CurrentBuildingInstance != null)
        {
            Object.Destroy(BuildingDataSO.CurrentBuildingInstance);
            SetBuildingInstance(null);
//            _currentBuildingInstance = null;
        }
    }

    private bool CanBuildHere()
    {
        Collider[] collidersArray = Physics.OverlapBox(
            BuildingDataSO.SelectedBuildingIcon.transform.position,
            BuildingDataSO.SelectedBuildingIcon.transform.localScale,
//            Quaternion.identity, 
            BuildingDataSO.SelectedBuildingIcon.transform.localRotation, 
            ~BuildingDataSO.GroundLayerMask);

        // TODO - Do this better. 
        // Remove collisions with self.
        List<Collider> collidersList = collidersArray
            .Where(collider => collider.gameObject.GetInstanceID() == BuildingDataSO.SelectedBuildingIcon.gameObject.GetInstanceID()).ToList();
//        List<Collider> collidersList = collidersArray.ToList();
        for (int i = collidersList.Count - 1; i >= 0; i--)
        {
            //Debug.Log(_currentBuildingInstance.transform.GetChild(0).gameObject.GetInstanceID() + " collided with " + collider.gameObject.GetInstanceID());

//            if (BuildingDataSO.SelectedBuildingIcon.gameObject.GetInstanceID() == collidersList[i].gameObject.GetInstanceID())
            {
                collidersList.RemoveAt(i);
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