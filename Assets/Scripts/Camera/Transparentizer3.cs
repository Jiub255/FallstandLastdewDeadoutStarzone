using System.Collections.Generic;
using UnityEngine;
/*using UnityEngine.EventSystems;
using UnityEngine.InputSystem;*/

// TODO - Keep track of which materials just stopped being in the way, like Transparentizer, and reset them. 
// Not sure if this will work, making all walls fade, not just the ones in the way. 
public class Transparentizer3 : MonoBehaviour
{
    [SerializeField, Range(0f, 5f)]
    private float _size = 1f;
    [SerializeField, Range(0f, 1f)]
    private float _smoothness = 0.5f;
    [SerializeField, Range(0f, 1f)]
    private float _opacity = 1f;
	[SerializeField]
	private LayerMask _transparentableLayerMask;

    // Just for testing/getting values right. 
    // TODO - Hopefully find a better fix for the PC box/sphere/ray cast eventually. 
    [SerializeField]
    private float _boxcastHalfWidth = 50f;
    [SerializeField]
    private float _boxcastHalfHeight = 50f;

    private Transform _transform;
	private Transform _currentPCTransform;
	private Camera _camera;
    private List<Material> _fadedMaterials = new();
/*    private InputAction _mousePositionAction;
    private EventSystem _eventSystem;
    private bool _pointerOverUI = false;*/

    private int _pcPositionID = Shader.PropertyToID("_PCPosition");
    private int _sizeID = Shader.PropertyToID("_Size");
    private int _smoothnessID = Shader.PropertyToID("_Smoothness");
    private int _opacityID = Shader.PropertyToID("_Opacity");

    private void Start()
    {
/*        _mousePositionAction = S.I.IM.PC.Camera.MousePosition;
        _eventSystem = EventSystem.current;*/
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

/*    private void Update()
    {
        if (_eventSystem.IsPointerOverGameObject())
        {
            _pointerOverUI = true;
        }
        else
        {
            _pointerOverUI = false;
        }
    }*/

    // TODO - How to handle mouse position? Add to the shader graph? 
    private void FixedUpdate()
    {
        // Get list of new in the way materials.
        if (_currentPCTransform != null)
        {
            // Hits from currently selected PC 
            Collider[] colliders;

            // Halfway point between camera and PC for the box center. 
            Vector3 center = (_transform.position + _currentPCTransform.position) / 2f;

            float boxHalfDepth = (_currentPCTransform.position - _transform.position).magnitude / 2f;

            Vector3 halfExtents = new Vector3(
                _boxcastHalfWidth,
                _boxcastHalfHeight,
                boxHalfDepth);

            Quaternion orientation = Quaternion.LookRotation(_currentPCTransform.position - _transform.position, Vector3.up);

            // Overlap box is cutting off parts of corner walls to the side of PC, somehow use a different shape/cast?
            // Ideally some sort of OverlapTetrahedron from camera bounds to PC would work. How to do something like this? 
            colliders = Physics.OverlapBox(
                center,
                halfExtents,
                orientation,
                _transparentableLayerMask);

            Debug.Log($"# of hits: {colliders.Length}, half extents: {halfExtents}");

            // Set PC screen position. 
            Vector3 pcPosition = _camera.WorldToViewportPoint(_currentPCTransform.position);

            // Get all in the way materials. 
            List<Material> inTheWayMaterials = new();
            foreach (Collider collider in colliders)
            {
                Renderer[] renderers = collider.transform.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    inTheWayMaterials.AddRange(renderer.materials);
                }
            }

            // For each one, set its properties, and add it to the list. 
            foreach (Material material in inTheWayMaterials)
            {
//                if (!_fadedMaterials.Contains(material))
                {
                    material.SetVector(_pcPositionID, pcPosition);
                    material.SetFloat(_sizeID, _size);
                    material.SetFloat(_smoothnessID, _smoothness);
                    material.SetFloat(_opacityID, _opacity);

                    _fadedMaterials.Add(material);
                }
            }

            // Get all items on faded materials that aren't on the in the way materials list (so they just stopped being in the way), 
            // and reset their properties (just set size to 0f, the others don't matter). 
            // Also remove material from _fadedMaterials. 
            for (int i = _fadedMaterials.Count - 1; i >= 0; i--)
            {
                if (!inTheWayMaterials.Contains(_fadedMaterials[i]))
                {
                    _fadedMaterials[i].SetFloat(_sizeID, 0f);

                    _fadedMaterials.RemoveAt(i);
                }
            }
        }
        else
        {
            // Reset all faded materials. 
            if (_fadedMaterials.Count > 0)
            {
                for (int i = _fadedMaterials.Count - 1; i >= 0; i--)
                {
                    _fadedMaterials[i].SetFloat(_sizeID, 0f);

                    _fadedMaterials.RemoveAt(i);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_currentPCTransform != null)
        {
            // Halfway point between camera and PC for the box center. 
            Vector3 center = (_transform.position + _currentPCTransform.position) / 2f;

            float boxHalfDepth = (_currentPCTransform.position - _transform.position).magnitude / 2f;

            Vector3 halfExtents = new Vector3(
                _boxcastHalfWidth,
                _boxcastHalfHeight,
                boxHalfDepth);

            Quaternion orientation = Quaternion.LookRotation(_currentPCTransform.position - _transform.position, Vector3.up);

            // create a matrix which translates an object by "position", rotates it by "rotation" and scales it by "halfExtends * 2"
            Gizmos.matrix = Matrix4x4.TRS(center, orientation, halfExtents * 2);
            // Then use it one a default cube which is not translated nor scaled
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}