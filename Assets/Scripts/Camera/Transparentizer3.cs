using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// TODO - Keep track of which materials just stopped being in the way, like Transparentizer, and reset them. 
// Not sure if this will work, making all walls fade, not just the ones in the way. 
public class Transparentizer3 : MonoBehaviour
{
    /*private*/ public static int _pcPositionID = Shader.PropertyToID("_PCPosition");
    /*private*/ public static int _sizeID = Shader.PropertyToID("_Size");
    /*private*/ public static int _smoothnessID = Shader.PropertyToID("_Smoothness");
    /*private*/ public static int _opacityID = Shader.PropertyToID("_Opacity");

	private Transform _currentPCTransform;

	[SerializeField]
	private LayerMask _transparentableLayerMask;
    [SerializeField, Range(0f, 5f)]
    private float _size = 1f;
    [SerializeField, Range(0f, 1f)]
    private float _smoothness = 0.5f;
    [SerializeField, Range(0f, 1f)]
    private float _opacity = 1f;

	private Camera _camera;
    private InputAction _mousePositionAction;
    private bool _pointerOverUI = false;
    private EventSystem _eventSystem;
    private Transform _transform;
    private RaycastHit[] _oldHits = new RaycastHit[0];
    private bool _materialsReset = false;

    private void Start()
    {
        _mousePositionAction = S.I.IM.PC.Camera.MousePosition;
        _eventSystem = EventSystem.current;
        _transform = transform;
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        PCSelector.OnSelectPC += SelectPC;
    }

    private void OnDisable()
    {
        PCSelector.OnSelectPC -= SelectPC;
    }

    private void SelectPC(Transform pcTransform)
    {
        _currentPCTransform = pcTransform;
    }

    private void Update()
    {
        if (_eventSystem.IsPointerOverGameObject())
        {
            _pointerOverUI = true;
        }
        else
        {
            _pointerOverUI = false;
        }
    }

    // TODO - How to handle mouse position? Add to the shader graph? 
    private void FixedUpdate()
    {
        // TODO - Keep track of which materials just stopped being in the way, like Transparentizer, and reset them. 
        // Get list of new in the way materials.
        // Set their properties. 
        // Get all items on old in the way materials that aren't on the new list, so they just stopped being in the way. 
        // Reset their properties. 


        RaycastHit[] newHits = new RaycastHit[0];

        // Hits from mouse position 
/*        if (!_pointerOverUI)
        {
            hits = Physics.RaycastAll(
                _camera.ScreenPointToRay(_mousePositionAction.ReadValue<Vector2>()),
                1000,
                _transparentableLayerMask);
        }*/

        // Hits from currently selected PC 
        if (_currentPCTransform != null)
        {
            Vector3 position = _transform.position;
            Vector3 direction = _currentPCTransform.position - position;
//            float rayDistance = Vector3.Distance(position, _currentPCTransform.position);

            /*RaycastHit[] selectedPlayerHits*/ newHits = Physics.SphereCastAll(
                position,
                // Might have to adjust this to be the correct size. 
                _size, 
                direction,
                direction.magnitude,
                _transparentableLayerMask);

            // Combine all hits into one array. 
//            hits = selectedPlayerHits.Concat(hits).ToArray();

            // Set PC screen position. 
            Vector3 pcPosition = _camera.WorldToViewportPoint(_currentPCTransform.position);

            foreach (RaycastHit hit in newHits)
            {
                Renderer[] renderers = hit.transform.GetComponentsInChildren<Renderer>();
                List<Material> materials = new();
                foreach (Renderer renderer in renderers)
                {
                    materials.AddRange(renderer.materials);   
                }

                foreach (Material material in materials)
                {
                    Debug.Log($"{material.name} Size property: {material.GetFloat(_sizeID)}");
                    material.SetVector(_pcPositionID, pcPosition);
                    material.SetFloat(_sizeID, _size);
                    material.SetFloat(_smoothnessID, _smoothness);
                    material.SetFloat(_opacityID, _opacity);
                }
            }

            if (_materialsReset == true)
            {
                _materialsReset = false;
            }
        }
        else if (!_materialsReset)
        {
            if (_oldHits.Length > 0)
            {
                foreach(RaycastHit hit in _oldHits)
                {
                    Renderer[] renderers = hit.transform.GetComponentsInChildren<Renderer>();
                    List<Material> materials = new();
                    foreach (Renderer renderer in renderers)
                    {
                        materials.AddRange(renderer.materials);
                    }

                    foreach (Material material in materials)
                    {
                        material.SetFloat(_sizeID, 0f);
                        Debug.Log("Materials reset."); 
                    }
                }
            }

            _materialsReset = true;
        }

        _oldHits = newHits;
    }
}