using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLeader : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField]
    [Range(0f, 15f)]
    private float zoomSpeed = 5f;

    [SerializeField]
    [Range(0f, 10f)]
    private float zoomMinDist = 3f;

    [SerializeField]
    [Range(15f, 255f)]
    private float zoomMaxDist = 100f;

    [Header("WASD/Arrow Key Movement")]
    [SerializeField]
    [Range(0f, 30f)]
    private float movementSpeed = 10f;

    private Vector3 forward;
    private Vector3 right;

    [Header("Drag Screen")]
    [SerializeField]
    private LayerMask groundLayer;

    private Vector3 startDrag;

    [Header("Rotate Camera")]
    [SerializeField]
    [Range(0f, 10f)]
    private float rotationSpeed = 0.75f;

    [SerializeField]
    [Range(0f, 40f)]
    private float rotationXMin = 7f;

    [SerializeField]
    [Range(50f, 90f)]
    private float rotationXMax = 90f;

    [SerializeField]
    private Transform rotationOrigin;

#if !UNITY_EDITOR
    [Header("Edge Scrolling")]
    [SerializeField]
    [Range(0f, 30f)]
    private float edgeScrollingSpeed = 10f;

    [Tooltip("Calculated using percent of width")]
    [SerializeField]
    [Range(0f, 30f)]
    private float percentDistanceFromEdges = 10f;

    private float screenWidth;
    private float screenHeight;
    private float edgeDistance;

    private void Awake()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        edgeDistance = screenWidth * (percentDistanceFromEdges / 100);
    }
#endif

    private void Start()
    {
        S.I.InputManager.playerControls.World.Zoom.performed += Zoom;
    }

    private void OnDisable()
    {
        S.I.InputManager.playerControls.World.Zoom.performed -= Zoom;
    }

    private void Zoom(InputAction.CallbackContext context)
    {
        float wheelMovement = S.I.InputManager.playerControls.World.Zoom.ReadValue<float>();
        Vector3 cameraZoomMovement = /*cam.*/transform.forward * wheelMovement * zoomSpeed * Time.deltaTime;
        float modifiedLocalZ = rotationOrigin.localPosition.z + cameraZoomMovement.magnitude * -Mathf.Sign(wheelMovement);

        // Clamp zoom between min and max distances
        // Works, but not perfectly. Want to lerp the zoom so it's smoother and then set it to min/max zoom if it crosses those thresholds
        if (modifiedLocalZ > zoomMinDist && modifiedLocalZ < zoomMaxDist)
        {
            // Move camera leader (main camera follows it smoothly)
            transform.position += cameraZoomMovement;

            // Change the rotation origin child's local z-component so it doesn't move in world space
            rotationOrigin.localPosition = new Vector3(
                rotationOrigin.localPosition.x,
                rotationOrigin.localPosition.y,
                modifiedLocalZ);
        }
    }

    private void LateUpdate()
    {
        // Zoom overrides everything else. Not really noticeable since this action gets called only during
        // isolated frames, but it helps resolve some issues with moving while zooming.
        if (!S.I.InputManager.playerControls.World.Zoom.WasPerformedThisFrame())
        {
            //--------------------------------------------------------

            // GET CAMERA LEADER FORWARD AND RIGHT VECTORS
            forward = transform.forward;
            right = transform.right;

            // Project the forward and right vectors onto the horizontal plane (y = 0)
            forward.y = 0f;
            right.y = 0f;

            // Normalize them
            forward.Normalize();
            right.Normalize();

            //--------------------------------------------------------

            // MOVE CAMERA USING WASD/ARROW KEYS
            // Get input
            Vector2 movement =
                S.I.InputManager.playerControls.World.MoveCamera.ReadValue<Vector2>();

            // Translate movement vector to world space
            Vector3 keyboardMovement = (forward * movement.y) + (right * movement.x);

            // Move
            transform.position += keyboardMovement * movementSpeed * Time.deltaTime;

            //--------------------------------------------------------

            // DRAG CAMERA WHILE RIGHT MOUSE BUTTON HELD DOWN
            // But not when rotate is held too. Rotate overrides drag.
            if (S.I.InputManager.playerControls.World.DragCamera.IsPressed() &&
                !S.I.InputManager.playerControls.World.RotateCamera.IsPressed())
            {
                Ray ray = Camera.main.ScreenPointToRay(
                    S.I.InputManager.playerControls.World.MousePosition.ReadValue<Vector2>());
                RaycastHit hitData;
                // If you click on ground (as in not off screen/off the terrain), ...
                if (Physics.Raycast(ray, out hitData, 1000, groundLayer))
                {
                    if (S.I.InputManager.playerControls.World.DragCamera.WasPressedThisFrame())
                    {
                        // Get the point on the ground where you originally clicked.
                        // Only happens the first frame you click.
                        startDrag = ray.GetPoint(hitData.distance);
                    }
                    else
                    {
                        // Move camera leader the opposite direction you move mouse
                        transform.position += startDrag - ray.GetPoint(hitData.distance);
                    }
                }
            }

            //--------------------------------------------------------

            // ROTATE CAMERA WHILE MOUSE WHEEL BUTTON HELD DOWN
            if (S.I.InputManager.playerControls.World.RotateCamera.IsPressed())
            {
                // Rotation around y-axis
                float deltaX =
                    S.I.InputManager.playerControls.World.MouseDelta.ReadValue<Vector2>().x *
                    rotationSpeed;

                transform.RotateAround(rotationOrigin.position, Vector3.up, deltaX);

                // TODO: Try using transform.rotation.GetAngleAxis 
                // Rotation around axis parallel to your local right vector, this axis always parallel to xz-plane.
                Vector3 axis = new Vector3(
                    -Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y),
                    0f,
                    Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y));

                float deltaY =
                    S.I.InputManager.playerControls.World.MouseDelta.ReadValue<Vector2>().y *
                    rotationSpeed;

                // Clamp x-rotation between min and max values (at most 0 - 90).
                if (transform.rotation.eulerAngles.x - deltaY > rotationXMin && transform.rotation.eulerAngles.x - deltaY <= rotationXMax)
                {
                    transform.RotateAround(rotationOrigin.position, axis, deltaY);
                }
            }

#if !UNITY_EDITOR
            //--------------------------------------------------------

            // EDGE SCROLLING BASED ON MOUSE POSITION
            // Don't edge scroll if holding down mouse wheel button
            else
            {
                // Get mouse screen position
                Vector3 mousePos =
                    S.I.InputManager.playerControls.World.MousePosition.ReadValue<Vector2>();

                int mouseX = 0;
                int mouseY = 0;

                // Check if mouse screen position is near the edges
                if (mousePos.x > screenWidth - edgeDistance)
                {
                    mouseX = 1;
                }
                else if (mousePos.x < edgeDistance)
                {
                    mouseX = -1;
                }

                if (mousePos.y > screenHeight - edgeDistance)
                {
                    mouseY = 1;
                }
                else if (mousePos.y < edgeDistance)
                {
                    mouseY = -1;
                }

                // Direction we want to move
                Vector3 edgeScrollMovement = (forward * mouseY) + (right * mouseX);

                // Move camera
                transform.position += edgeScrollMovement * edgeScrollingSpeed * Time.deltaTime;
            }
#endif
        }
    }
}