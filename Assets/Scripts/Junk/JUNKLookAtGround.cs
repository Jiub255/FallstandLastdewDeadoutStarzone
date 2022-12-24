using UnityEngine;
using UnityEngine.InputSystem;

public class JUNKLookAtGround : MonoBehaviour
{
/*    [Header("Feed this with the world focus point.")]
    public Transform FocusTransform;

    [Header("How far back from the point.")]
    [Range(5, 40)]
    public float Distance = 10;

    [Header("Heading around the compass")]
    public float Heading;

    [Header("Regard angle: high is overhead.")]
    [Range(10, 80)]
    public float Angle = 45;

    [Header("This can be handy: to lift a bit.")]
    public float Raise = 2.0f;

    //----------------------------------------

    [SerializeField]
    private float movementSpeed = 10f;

    [SerializeField]
    private float rotationSpeed = 0.75f;

    [SerializeField]
    private float zoomSpeed = 5f;

    [SerializeField]
    private float edgeScrollingSpeed = 10f;

    [Header("Calculated using percent of width")]
    [SerializeField]
    private float percentDistanceFromEdges = 10f;

    private Camera cam;
    private float screenWidth;
    private float screenHeight;
    private float edgeDistance;

    // NEW INPUT SYSTEM STUFF
    private PlayerInput playerInput;

    private InputAction mousePositionAction;
    private InputAction moveCameraAction;
    private InputAction rotateCameraAction;
    private InputAction mouseDeltaAction;
    private InputAction mouseWheelAction;

    private bool wheelHeld = false;

    private Vector3 forward;
    private Vector3 right;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        edgeDistance = screenWidth * (percentDistanceFromEdges / 100);

        playerInput = GetComponent<PlayerInput>();
        mousePositionAction = playerInput.actions["MousePosition"];
        moveCameraAction = playerInput.actions["MoveCamera"];
        rotateCameraAction = playerInput.actions["RotateCamera"];
        mouseDeltaAction = playerInput.actions["MouseDelta"];
        mouseWheelAction = playerInput.actions["MouseWheel"];
    }

    private void OnEnable()
    {
        rotateCameraAction.started += WheelPressed;
        rotateCameraAction.canceled += WheelReleased;
        mouseWheelAction.performed += Zoom;
    }

    private void OnDisable()
    {
        rotateCameraAction.started -= WheelPressed;
        rotateCameraAction.canceled -= WheelReleased;
        mouseWheelAction.performed -= Zoom;
    }

    private void WheelPressed(InputAction.CallbackContext context)
    {
        Debug.Log("WheelPressed called");

        wheelHeld = true;
    }

    private void WheelReleased(InputAction.CallbackContext context)
    {
        Debug.Log("WheelReleased called");

        wheelHeld = false;
    }

    private void Zoom(InputAction.CallbackContext context)
    {
        Distance += mouseWheelAction.ReadValue<float>() * zoomSpeed * Time.deltaTime;

        *//*transform.position += cam.transform.forward * mouseWheelAction.ReadValue<float>() * 
            zoomSpeed * Time.deltaTime;
        FocusTransform.position -= cam.transform.forward * mouseWheelAction.ReadValue<float>() * 
            zoomSpeed * Time.deltaTime;*//*
    }

    private void Update()
    {
        // GET CAMERA VECTORS
        forward = cam.transform.forward;
        right = cam.transform.right;

        // Project the forward and right vectors onto the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();


        // MOVE CAMERA USING WASD/ARROW KEYS
        // Get input
        Vector2 movement = moveCameraAction.ReadValue<Vector2>();

        // This is the direction in the world space we want to move
        Vector3 keyboardMovement = (forward * movement.y) + (right * movement.x);

        // Apply the movement
        FocusTransform.position += keyboardMovement * movementSpeed * Time.deltaTime;


        // ROTATE CAMERA WHILE MOUSE WHEEL BUTTON HELD DOWN
        if (wheelHeld)
        {
            Heading += mouseDeltaAction.ReadValue<Vector2>().x * rotationSpeed;
            Angle += mouseDeltaAction.ReadValue<Vector2>().y * rotationSpeed;

            *//*Debug.Log("MouseDeltaX: " + mouseDeltaAction.ReadValue<Vector2>().x 
                            + ", MouseDeltaY: " + mouseDeltaAction.ReadValue<Vector2>().y);*/

/*            float deltaX = mouseDeltaAction.ReadValue<Vector2>().x * rotationSpeed;
            transform.RotateAround(FocusTransform.position, Vector3.up, deltaX);

            Vector3 axis = new Vector3(
                -Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y),
                0f,
                Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y));

            float deltaY = mouseDeltaAction.ReadValue<Vector2>().y * rotationSpeed;
            transform.RotateAround(FocusTransform.position, axis, deltaY);*//*
        }


//#if !UNITY_EDITOR
        // EDGE SCROLLING BASED ON MOUSE POSITION
        // Don't edge scroll if holding down mouse wheel button
        else if (!wheelHeld)
        {
            // Get mouse screen position
            Vector3 mousePos = mousePositionAction.ReadValue<Vector2>();

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
            FocusTransform.position += edgeScrollMovement * edgeScrollingSpeed * Time.deltaTime;
        }
//#endif
    }

    void LateUpdate()
    {
        // first around the heading
        Quaternion rot = Quaternion.Euler(0, Heading, 0);

        // next our regard angle
        rot = rot * Quaternion.Euler(-Angle, 0, 0);

        // and back up vector
        Vector3 backup = rot * Vector3.forward;

        // stare point could be up a bit from the ground
        Vector3 StarePoint = FocusTransform.position + Vector3.up * Raise;

        // bring it all together
        Vector3 ViewpointPosition = StarePoint + backup * Distance;

        // make it so
        transform.position = ViewpointPosition;
        transform.LookAt(StarePoint);
    }*/
}
