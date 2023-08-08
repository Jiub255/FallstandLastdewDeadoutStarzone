using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Put this on Camera Leader
public class CameraZoom : MonoBehaviour
{
    public static event Action<float> OnZoomChanged;

    [SerializeField, Range(0f, 10f)]
    private float _zoomSpeed = 2f;

    [SerializeField, Range(0f, 15f)]
    private float _zoomMinDist = 10f;

    [SerializeField, Range(16f, 255f)]
    private float _zoomMaxDist = 50f;

    [SerializeField, Range(5f, 35f)]
    private float _defaultZoomDist = 15f;

    private Transform _transform;
    private InputAction _zoomAction;
    private float _zoomLevel;

    // Just doing this to make for better "speed" values in the inspector. 
    private float ZoomSpeed { get { return _zoomSpeed / 200f; } }

    private void OnEnable()
    {
        _transform = transform;
        GameManager.OnInputManagerCreated += SetupInput;

        CameraMoveRotate.OnCenterOnPC += () => ZoomTo(_defaultZoomDist);

        ZoomTo(_defaultZoomDist);
    }

    private void SetupInput(InputManager inputManager)
    {
        _zoomAction = inputManager.PC.Camera.Zoom;

        _zoomAction.performed += Zoom;
    }

    private void OnDisable()
    {
        GameManager.OnInputManagerCreated -= SetupInput;

        _zoomAction.performed -= Zoom;

        CameraMoveRotate.OnCenterOnPC -= () => ZoomTo(_defaultZoomDist);
    }

    private void ZoomTo(float zoomLevel)
    {
        _zoomLevel = zoomLevel;

        // Clamp zoom between min and max distances. 
        if (_zoomLevel < _zoomMinDist) _zoomLevel = _zoomMinDist;
        if (_zoomLevel > _zoomMaxDist) _zoomLevel = _zoomMaxDist;

        _transform.localPosition = new Vector3(0f, 0f, -zoomLevel);

        // Transparentizer listens, changes cutout size accordingly. 
        OnZoomChanged?.Invoke(zoomLevel);
    }
    
    private void Zoom(InputAction.CallbackContext context)
    {
        // Change zoom level based on mouse scroll wheel input. 
        ZoomTo(_zoomLevel - (_zoomAction.ReadValue<float>() * ZoomSpeed));
    }
}