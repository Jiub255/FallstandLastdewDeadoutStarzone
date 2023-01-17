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

    [SerializeField]
    private SelectedPCSO _selectedPCSO;

    private void Start()
    {
        S.I.IM.PC.World.Zoom.performed += Zoom;

        CameraMoveRotate.OnCenterOnPC += CenterOnPC;
    }

    private void OnDisable()
    {
        S.I.IM.PC.World.Zoom.performed -= Zoom;

        CameraMoveRotate.OnCenterOnPC -= CenterOnPC;
    }

    private void CenterOnPC()
    {
        // Set zoom to "default" setting.
        transform.localPosition = new Vector3(0f, 0f, -_defaultZoomDist);
    }

    private void Zoom(InputAction.CallbackContext context)
    {
        float wheelMovement = S.I.IM.PC.World.Zoom.ReadValue<float>();

        Vector3 cameraZoomMovement = Vector3.forward * wheelMovement * _zoomSpeed * Time.unscaledDeltaTime;

        float modifiedLocalZ = (transform.localPosition + cameraZoomMovement).z;

        // Clamp zoom between min and max distances
        if (modifiedLocalZ < -_zoomMinDist && modifiedLocalZ > -_zoomMaxDist)
        {
            // Move camera leader (main camera follows it smoothly)
            transform.localPosition += cameraZoomMovement;
        }
    }
}