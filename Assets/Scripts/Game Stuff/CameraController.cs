using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 10f;

    [SerializeField]
    private float rotationSpeed = 0.75f;

    [SerializeField]
    private float rotationXMin = 7f;

    [SerializeField]
    private float rotationXMax = 90f;

    [SerializeField]
    private float zoomSpeed = 5f;

    [SerializeField]
    private float zoomMinDist = 3f;

    [SerializeField]
    private float zoomMaxDist = 100f;

    [Tooltip("Calculated using percent of width")]
    [SerializeField]
    private float percentDistanceFromEdges = 10f;

    [SerializeField]
    private Transform rotationOrigin;

    [SerializeField]
    private LayerMask groundLayer;

    private Camera cam;

//#if !UNITY_EDITOR
    [SerializeField]
    private float edgeScrollingSpeed = 10f;

    private float screenWidth;
    private float screenHeight;
    private float edgeDistance;
//+#endif

    private Vector3 forward;
    private Vector3 right;

    private Vector3 startDrag;

    private bool clickedGround = false;

    private void Awake()
    {
        cam = GetComponent<Camera>();

//#if !UNITY_EDITOR
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        edgeDistance = screenWidth * (percentDistanceFromEdges / 100);
//#endif
    }

    private void Start()
    {
        MasterSingleton.Instance.InputManager.mouseWheelAction.performed += Zoom;
    }

    private void OnDisable()
    {
        MasterSingleton.Instance.InputManager.mouseWheelAction.performed -= Zoom;
    }

    // TODO: Make camera child of rotation origin, or make both children of empty parent. Might make this math easier.
    private void Zoom(InputAction.CallbackContext context)
    {
        float wheelMovement = MasterSingleton.Instance.InputManager.mouseWheelAction.ReadValue<float>();
        Vector3 cameraZoomMovement = cam.transform.forward * wheelMovement * zoomSpeed * Time.deltaTime;
        float modifiedLocalZ = rotationOrigin.localPosition.z + cameraZoomMovement.magnitude * -Mathf.Sign(wheelMovement);

        // Clamp zoom between min and max distances
        // Works, but not perfectly. Want to lerp the zoom so it's smoother and then set it to min/max zoom if it crosses those thresholds
        if (modifiedLocalZ > zoomMinDist && modifiedLocalZ < zoomMaxDist)
        {
            // Move camera (and rotation origin child object with it)
            transform.position += cameraZoomMovement;

            // Change the child's local z-component so it doesn't move in world space
            rotationOrigin.localPosition = new Vector3(
                rotationOrigin.localPosition.x,
                rotationOrigin.localPosition.y,
                modifiedLocalZ);
        }
    }

    private void LateUpdate()
    {
        if (!UIManager.gamePaused)
        {
            // Zoom overrides everything else. Not really noticeable since this action gets called only during
                // isolated frames, but it helps resolve some issues with moving while zooming.
            if (!MasterSingleton.Instance.InputManager.mouseWheelAction.WasPerformedThisFrame())
            {
                //--------------------------------------------------------
                // GET CAMERA VECTORS
                forward = cam.transform.forward;
                right = cam.transform.right;

                // Project the forward and right vectors onto the horizontal plane (y = 0)
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();

                //--------------------------------------------------------
                // MOVE CAMERA USING WASD/ARROW KEYS
                // Get input
                Vector2 movement = 
                    MasterSingleton.Instance.InputManager.moveCameraAction.ReadValue<Vector2>();

                // This is the direction in the world space we want to move
                Vector3 keyboardMovement = (forward * movement.y) + (right * movement.x);

                // Apply the movement
                transform.position += keyboardMovement * movementSpeed * Time.deltaTime;

                //--------------------------------------------------------
                // DRAG CAMERA WHILE RIGHT MOUSE BUTTON HELD DOWN
                // But not when rotate is held too. Rotate overrides drag.
                // TODO: Add a check for if ray hit ground on the first frame you clicked.
                // If not, set a bool to false so this section doesn't run until you release and try again.

                if (MasterSingleton.Instance.InputManager.mouseRightAction.WasPressedThisFrame()/* &&
                    !MasterSingleton.Instance.InputManager.rotateCameraAction.IsPressed()*/)
                {
                    Ray ray1 = Camera.main.ScreenPointToRay(
                        MasterSingleton.Instance.InputManager.mousePositionAction.ReadValue<Vector2>());
                    RaycastHit hitData1;
                    if (Physics.Raycast(ray1, out hitData1, 1000, groundLayer))
                    {
                        clickedGround = true;
                    }
                    else
                    {
                        clickedGround = false;
                    }
                }

                if (clickedGround)
                {
                    if (MasterSingleton.Instance.InputManager.mouseRightAction.IsPressed() && 
                        !MasterSingleton.Instance.InputManager.rotateCameraAction.IsPressed())
                    {
                        Ray ray = Camera.main.ScreenPointToRay(
                            MasterSingleton.Instance.InputManager.mousePositionAction.ReadValue<Vector2>());
                        RaycastHit hitData;
                        if (Physics.Raycast(ray, out hitData, 1000, groundLayer))
                        {
                            if (MasterSingleton.Instance.InputManager.mouseRightAction.WasPressedThisFrame())
                            {
                                startDrag = ray.GetPoint(hitData.distance);
                            }
                            else
                            {
                                transform.position += startDrag - ray.GetPoint(hitData.distance);
                            }
                        }
                    }
                }

                //--------------------------------------------------------
                // ROTATE CAMERA WHILE MOUSE WHEEL BUTTON HELD DOWN
                if (MasterSingleton.Instance.InputManager.rotateCameraAction.IsPressed())
                {
                    float deltaX = 
                        MasterSingleton.Instance.InputManager.mouseDeltaAction.ReadValue<Vector2>().x * 
                        rotationSpeed;

                    transform.RotateAround(rotationOrigin.position, Vector3.up, deltaX);

                    // TODO: Try using transform.rotation.GetAngleAxis 
                    Vector3 axis = new Vector3(
                        -Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y),
                        0f,
                        Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y));

                    float deltaY =
                        MasterSingleton.Instance.InputManager.mouseDeltaAction.ReadValue<Vector2>().y *
                        rotationSpeed;

                    // Clamp up/down rotation
                    if (transform.rotation.eulerAngles.x - deltaY > rotationXMin && transform.rotation.eulerAngles.x - deltaY <= rotationXMax)
                    {
                        transform.RotateAround(rotationOrigin.position, axis, deltaY);
                    }
                }

    //#if !UNITY_EDITOR
                //--------------------------------------------------------
                // EDGE SCROLLING BASED ON MOUSE POSITION
                // Don't edge scroll if holding down mouse wheel button
                else
                {
                    // Get mouse screen position
                    Vector3 mousePos = 
                        MasterSingleton.Instance.InputManager.mousePositionAction.ReadValue<Vector2>();

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
    //#endif
            }
        }
    }
}