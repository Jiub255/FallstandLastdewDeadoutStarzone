using UnityEngine;
using UnityEngine.InputSystem;

// Put this on Camera Leader
public class CameraZoom : MonoBehaviour
{
    [SerializeField, Range(0f, 40f)]
    private float _zoomSpeed = 20f;

    [SerializeField, Range(0f, 15f)]
    private float _zoomMinDist = 3f;

    [SerializeField, Range(16f, 255f)]
    private float _zoomMaxDist = 100f;

    [SerializeField, Range(5f, 35f)]
    private float _defaultZoomDist = 15f;

    private Transform _transform;
    private InputAction _zoomAction;

    private void Start()
    {
        _transform = transform;
        _zoomAction = S.I.IM.PC.Camera.Zoom;

        _zoomAction.performed += Zoom;

        CameraMoveRotate.OnCenterOnPC += CenterOnPC;
    }

    private void OnDisable()
    {
        _zoomAction.performed -= Zoom;

        CameraMoveRotate.OnCenterOnPC -= CenterOnPC;
    }

    private void CenterOnPC()
    {
        // Set zoom to "default" setting.
        _transform.localPosition = new Vector3(0f, 0f, -_defaultZoomDist);
    }

    private void Zoom(InputAction.CallbackContext context)
    {
        float wheelMovement = _zoomAction.ReadValue<float>();

        Vector3 cameraZoomMovement = Vector3.forward * wheelMovement * _zoomSpeed * Time.unscaledDeltaTime;

        float modifiedLocalZ = (_transform.localPosition + cameraZoomMovement).z;

        // Clamp zoom between min and max distances
        if (modifiedLocalZ < -_zoomMinDist && modifiedLocalZ > -_zoomMaxDist)
        {
            // Move camera leader (main camera follows it smoothly)
            _transform.localPosition += cameraZoomMovement;
        }
    }
}