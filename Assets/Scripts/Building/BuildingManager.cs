using UnityEngine;
using UnityEngine.InputSystem;

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

    private GameObject _currentBuildingPrefab;
    private GameObject _currentBuildingInstance;

    [SerializeField]
    private LayerMask _groundLayer;

    // For debug gizmos, so they dont draw in editor mode.
    //private bool _started;

    [SerializeField]
    private float _rotationSpeed = 50f;
    private Quaternion _rotation = Quaternion.identity;

    private bool _haveABuildingSelected = false;

    private void Start()
    {
        BuildingSO.OnSelectBuilding += SelectCurrentBuilding;

        S.I.IM.PC.MenuBuild.PlaceBuilding.performed += PlaceBuilding;
        S.I.IM.PC.WorldBuild.SnapBuilding.performed += SnapToNearest45;
        S.I.IM.PC.WorldBuild.DeselectBuilding.performed += DeselectCurrentBuilding;
        S.I.IM.PC.MenuBuild.CloseBuildMenu.performed += DeselectCurrentBuilding;

        // Don't see why this would ever be true, but just in case
        if (_currentBuildingInstance != null)
        {
            _haveABuildingSelected = true;
        }

        // For debug gizmos, so they dont draw in editor mode.
       // _started = true;
    }

    private void OnDisable()
    {
        BuildingSO.OnSelectBuilding -= SelectCurrentBuilding;

        S.I.IM.PC.MenuBuild.PlaceBuilding.performed -= PlaceBuilding;
        S.I.IM.PC.WorldBuild.SnapBuilding.performed -= SnapToNearest45;
        S.I.IM.PC.WorldBuild.DeselectBuilding.performed -= DeselectCurrentBuilding;
        S.I.IM.PC.MenuBuild.CloseBuildMenu.performed -= DeselectCurrentBuilding;
    }

    private void RotateBuilding()
    {
        _currentBuildingInstance.transform.Rotate(new Vector3(0f,
            Time.unscaledDeltaTime * _rotationSpeed * S.I.IM.PC.WorldBuild.RotateBuilding.ReadValue<float>(), 0f));

        _rotation = _currentBuildingInstance.transform.rotation;
    }

    private void SnapToNearest45(InputAction.CallbackContext context)
    {
        if (_currentBuildingInstance != null)
        {
            for (int i = 0; i <= 360; i += 45)
            {
                if (Mathf.Abs(_currentBuildingInstance.transform.rotation.eulerAngles.y - i) <= 22.5f)
                {
                    _currentBuildingInstance.transform.rotation = Quaternion.Euler(0f, i , 0f);
                }
            }
        }
    }

    private void Update()
    {
        if (_haveABuildingSelected)
        {
            RotateBuilding();
        }

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

            // Should only have null reference exception if the BuildingSO isn't set up properly (so not doing null check).
            _currentBuildingInstance = Instantiate(_currentBuildingPrefab);

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

    private void PlaceBuilding(InputAction.CallbackContext context)
    {
        if (_currentBuildingInstance != null)
        {
            // Nesting if because CanBuildHere needs _currentBuildingInstance to be not null.
            if (CanBuildHere())
            {
                // Turn off red/green highlights
                _currentBuildingInstance.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                _currentBuildingInstance.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

                // Collider not working unless I switch it off and on again.
                // Could make it disabled in prefab then just enable it after instantiating?
                _currentBuildingInstance.GetComponentInChildren<Collider>().enabled = true;

                // Stop controlling currentBuildingInstance with mouse, so essentially, build/set it.
                _currentBuildingInstance = null;

                // Make a new instance of same building to be the new currentBuildingInstance
                MakeInstance();
            }
        }
    }

    // Gets called from a button in build menu, which calls event in BuildingItem.
    // Also called when selecting a building the first time, not just changing buildings.
    public void SelectCurrentBuilding(GameObject newBuilding)
    {
        // Change from WorldGameplay to WorldBuild action map.
        S.I.IM.PC.WorldGameplay.Disable();
        S.I.IM.PC.WorldBuild.Enable();

       // Debug.Log("Changing current building to " + newBuilding.name);

        _currentBuildingPrefab = newBuilding;

        MakeInstance();
    }

    private void DeselectCurrentBuilding(InputAction.CallbackContext context)
    {
        // Change from WorldBuild to WorldGameplay action map.
        S.I.IM.PC.WorldBuild.Disable();
        S.I.IM.PC.WorldGameplay.Enable();

        if (_currentBuildingPrefab != null)
        {
            _currentBuildingPrefab = null;
        }
        if (_currentBuildingInstance != null)
        {
            Destroy(_currentBuildingInstance);
            _currentBuildingInstance = null;
        }
   
        _haveABuildingSelected = false;
    }

    private bool CanBuildHere()
    {
        BoxCollider boxCollider = _currentBuildingInstance.GetComponentInChildren<BoxCollider>();

        Collider[] colliders = Physics.OverlapBox(
            _currentBuildingInstance.transform.GetChild(0).transform.position,
            _currentBuildingInstance.transform.GetChild(0).transform.localScale, 
            Quaternion.identity, 
            ~_groundLayer);

/*        foreach (Collider collider in colliders)
        {
            Debug.Log(collider.gameObject.name);
        }*/

        // TODO: Might need to fix this after adding collider to building prefabs.
        // Stupid hack anyway, do it better. 
        // collides with itself for now?, cheap hack fix
        if (colliders.Length == 0)
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