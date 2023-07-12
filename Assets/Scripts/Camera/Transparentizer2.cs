using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Transparentizer2 : MonoBehaviour
{
	private Transform _currentPCTransform;

	[SerializeField]
	private LayerMask _transparentableLayerMask;
    [SerializeField]
    private float _cutoutSize = 0.1f;
    [SerializeField]
    private float _falloffSize = 0.05f;

	private Camera _camera;
    private InputAction _mousePositionAction;
    private bool _pointerOverUI = false;
    private EventSystem _eventSystem;
    private Transform _transform;
    private Vector2 _cutoutPosition;

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

    private void FixedUpdate()
    {
        RaycastHit[] hits = new RaycastHit[0];

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

            /*RaycastHit[] selectedPlayerHits*/ hits = Physics.RaycastAll(
                position,
                direction,
                direction.magnitude,
                _transparentableLayerMask);

            // Combine all hits into one array. 
//            hits = selectedPlayerHits.Concat(hits).ToArray();

            // Set cutout position. 
            _cutoutPosition = _camera.WorldToViewportPoint(_currentPCTransform.position);
            _cutoutPosition.y /= (Screen.width / Screen.height);
        }


        foreach (RaycastHit hit in hits)
        {
            Renderer[] renderers = hit.transform.GetComponentsInChildren<Renderer>();
            List<Material> materials = new();
            foreach (Renderer renderer in renderers)
            {
                materials.AddRange(renderer.materials);   
            }

            foreach (Material material in materials)
            {
                material.SetVector("_CutoutPosition", _cutoutPosition);
                material.SetFloat("_CutoutSize", _cutoutSize);
                material.SetFloat("_FalloffSize", _falloffSize);
            }
        }
    }
}