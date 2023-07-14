using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// TODO - Use a "Building" prefab with the selected building icon child. Then attach the actual building prefab as a child of it in this script.
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
    private GameObject _currentBuildingPrefab;
    private GameObject _currentBuildingInstance;
    private SelectedBuildingIcon _selectedBuildingIcon;

    [SerializeField]
    private LayerMask _groundLayerMask;

    // For debug gizmos, so they dont draw in editor mode.
    //private bool _started;

    [SerializeField]
    private float _rotationSpeed = 50f;
    private Quaternion _rotation = Quaternion.identity;

    private bool _haveABuildingSelected = false;
    private bool _pointerOverUI = false;
    private EventSystem _eventSystem;

    [SerializeField, Tooltip("Snap angle to the nearest [Snap Angle]. Works best with a divisor of 360.")]
    private int _snapAngle = 45;

    private InputAction _rotateBuildingAction;

    [SerializeField]
    protected SOInventory _craftingInventorySO;

    private void Start()
    {
        _rotateBuildingAction = S.I.IM.PC.Build.RotateBuilding;

        SOBuildingRecipe.OnSelectBuilding += SelectCurrentBuilding;

        S.I.IM.PC.Build.PlaceBuilding.performed += PlaceBuilding;
        S.I.IM.PC.Build.SnapBuilding.performed += SnapToNearestAngle;
        InputManager.OnDeselectOrCancel += DeselectCurrentBuilding;
        S.I.IM.PC.NonCombatMenus.CloseBuildMenu.performed += DeselectCurrentBuilding;

        UIBuilding.OnGetHaveEnoughItemsRecipes += GetHaveEnoughItemsRecipes;

        // Don't see why this would ever be true, but just in case. 
        if (_currentBuildingInstance != null)
        {
            _haveABuildingSelected = true;
        }

        _eventSystem = EventSystem.current;
        // For debug gizmos, so they dont draw in editor mode.
       // _started = true;
    }

    private void OnDisable()
    {
        SOBuildingRecipe.OnSelectBuilding -= SelectCurrentBuilding;

        S.I.IM.PC.Build.PlaceBuilding.performed -= PlaceBuilding;
        S.I.IM.PC.Build.SnapBuilding.performed -= SnapToNearestAngle;
        InputManager.OnDeselectOrCancel -= DeselectCurrentBuilding;
        S.I.IM.PC.NonCombatMenus.CloseBuildMenu.performed -= DeselectCurrentBuilding;

        UIBuilding.OnGetHaveEnoughItemsRecipes -= GetHaveEnoughItemsRecipes;
    }

    private List<SORecipe> GetHaveEnoughItemsRecipes(List<SORecipe> metRequirementsRecipes)
    {
        List<SORecipe> haveEnoughItemsRecipes = new();

        foreach (SORecipe recipe in metRequirementsRecipes)
        {
            foreach (RecipeCost recipeCost in recipe.RecipeCosts)
            {
                // If you don't have enough items to craft the recipe,  
                if (_craftingInventorySO.Contains(recipeCost.CraftingItemSO, recipeCost.Amount) == null)
                {
                    // Then go to next recipe. 
                    break;
                }
            }

            // Can only reach this point if you have at least recipeCost.Amount of each recipeCost.CraftingItemSO in your inventory.
            haveEnoughItemsRecipes.Add(recipe);
        }

        return haveEnoughItemsRecipes;
    }

    private void FixedUpdate()
    {
        if (_haveABuildingSelected)
        {
            RotateBuilding();

            // Move building to current mouse position on ground
            Ray ray = Camera.main.ScreenPointToRay(S.I.IM.PC.Camera.MousePosition.ReadValue<Vector2>());
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

    private void RotateBuilding()
    {
        if (_currentBuildingInstance != null)
        {
            _currentBuildingInstance.transform.Rotate(new Vector3(
                0f,
                Time.unscaledDeltaTime * _rotationSpeed * _rotateBuildingAction.ReadValue<float>(), 
                0f));

            _rotation = _currentBuildingInstance.transform.rotation;

            SetHighlight();
        }
    }

    private void SnapToNearestAngle(InputAction.CallbackContext context)
    {
        if (_currentBuildingInstance != null)
        {
            for (int i = 0; i <= 360; i += _snapAngle)
            {
                if (Mathf.Abs(_currentBuildingInstance.transform.rotation.eulerAngles.y - i) <= (_snapAngle / 2f))
                {
                    _currentBuildingInstance.transform.rotation = Quaternion.Euler(0f, i , 0f);
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
        if (_currentBuildingPrefab != null)
        {
            if (_currentBuildingInstance != null)
            {
                Destroy(_currentBuildingInstance);
                SetBuildingInstance(null);
            }

            SetBuildingInstance(Instantiate(_currentBuildingPrefab));
//            _currentBuildingInstance = Instantiate(_currentBuildingPrefab);

            // Set building's rotation
            _currentBuildingInstance.transform.rotation = _rotation;

            // Put new building in center of screen
            /*            Ray ray = Camera.main.ScreenPointToRay(
                            new Vector3(Screen.width / 2, Screen.height / 2, 0f));
                        RaycastHit hitData;
                        if (Physics.Raycast(ray, out hitData, 1000, _groundLayer))
                        {
                            _currentBuildingInstance.transform.position = hitData.point;
                        }*/

            _haveABuildingSelected = true;
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
        // Not sure what that means, look into it. 
        if (_currentBuildingInstance != null && !_pointerOverUI)
        {
            // Nesting if because CanBuildHere needs _currentBuildingInstance to be not null.
            if (CanBuildHere())
            {
                // TODO: Set this up better, don't use GetChild, use GetComponent or something. 
                // Turn off red/green highlights
//                _selectedBuildingIcon.DeactivateIcon();

/*                _currentBuildingInstance.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                _currentBuildingInstance.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);*/

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
    public void SelectCurrentBuilding(GameObject newBuilding)
    {
       // Debug.Log("Changing current building to " + newBuilding.name);

        _currentBuildingPrefab = newBuilding;

        MakeInstance();
    }

    private void DeselectCurrentBuilding(InputAction.CallbackContext context)
    {
        if (_currentBuildingPrefab != null)
        {
            _currentBuildingPrefab = null;
        }
        if (_currentBuildingInstance != null)
        {
            Destroy(_currentBuildingInstance);
            SetBuildingInstance(null);
//            _currentBuildingInstance = null;
        }

        _haveABuildingSelected = false;
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