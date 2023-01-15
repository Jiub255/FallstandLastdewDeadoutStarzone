using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLeader : MonoBehaviour
{
    [SerializeField, Range(0f, 15f), Header("Zoom")]
    private float _zoomSpeed = 5f;

    [SerializeField, Range(0f, 10f)]
    private float _zoomMinDist = 3f;

    [SerializeField, Range(15f, 255f)]
    private float _zoomMaxDist = 100f;

    [SerializeField, Range(0f, 30f), Header("Keyboard Movement")]
    private float _movementSpeed = 10f;

    private Vector3 _forward;
    private Vector3 _right;

    [SerializeField, Header("Drag Screen")]
    private LayerMask _groundLayer;

    private Vector3 _startDrag;

    [SerializeField, Range(0f, 10f), Header("Rotate Camera")]
    private float _rotationSpeed = 0.75f;

    [SerializeField, Range(0f, 40f)]
    private float _rotationXMin = 7f;

    [SerializeField, Range(50f, 90f)]
    private float _rotationXMax = 90f;

    [SerializeField]
    private Transform _rotationOrigin;

//#if !UNITY_EDITOR
    [SerializeField, Range(0f, 30f), Header("Edge Scrolling")]
    private float _edgeScrollingSpeed = 10f;

    [SerializeField, Range(0f, 30f), Tooltip("Calculated using percent of width")]
    private float _percentDistanceFromEdges = 10f;

    private float _screenWidth;
    private float _screenHeight;
    private float _edgeDistance;

    private void Awake()
    {
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
        _edgeDistance = _screenWidth * (_percentDistanceFromEdges / 100);
    }
//#endif

    private void Start()
    {
        S.I.IM.PC.WorldGameplay.Zoom.performed += Zoom;
    }

    private void OnDisable()
    {
        S.I.IM.PC.WorldGameplay.Zoom.performed -= Zoom;
    }

    private void Zoom(InputAction.CallbackContext context)
    {
        float wheelMovement = S.I.IM.PC.WorldGameplay.Zoom.ReadValue<float>();
        Vector3 cameraZoomMovement = /*cam.*/transform.forward * wheelMovement * _zoomSpeed * Time.deltaTime;
        float modifiedLocalZ = _rotationOrigin.localPosition.z + cameraZoomMovement.magnitude * -Mathf.Sign(wheelMovement);

        // Clamp zoom between min and max distances
        // Works, but not perfectly. Want to lerp the zoom so it's smoother and then set it to min/max zoom if it crosses those thresholds
        if (modifiedLocalZ > _zoomMinDist && modifiedLocalZ < _zoomMaxDist)
        {
            // Move camera leader (main camera follows it smoothly)
            transform.position += cameraZoomMovement;

            // Change the rotation origin child's local z-component so it doesn't move in world space
            _rotationOrigin.localPosition = new Vector3(
                _rotationOrigin.localPosition.x,
                _rotationOrigin.localPosition.y,
                modifiedLocalZ);
        }
    }

    private void LateUpdate()
    {
        // Zoom overrides everything else. Not really noticeable since this action gets called only during
        // isolated frames, but it helps resolve some issues with moving while zooming.
        if (!S.I.IM.PC.WorldGameplay.Zoom.WasPerformedThisFrame())
        {
            //--------------------------------------------------------

            // GET CAMERA LEADER FORWARD AND RIGHT VECTORS
            _forward = transform.forward;
            _right = transform.right;

            // Project the forward and right vectors onto the horizontal plane (y = 0)
            _forward.y = 0f;
            _right.y = 0f;

            // Normalize them
            _forward.Normalize();
            _right.Normalize();

            //--------------------------------------------------------

            // MOVE CAMERA USING WASD/ARROW KEYS
            // Get input
            Vector2 movement =
                S.I.IM.PC.World.MoveCamera.ReadValue<Vector2>();

            // Translate movement vector to world space
            Vector3 keyboardMovement = (_forward * movement.y) + (_right * movement.x);

            // Move
            transform.position += keyboardMovement * _movementSpeed * Time.deltaTime;

            //--------------------------------------------------------

            // DRAG CAMERA WHILE RIGHT MOUSE BUTTON HELD DOWN
            // But not when rotate is held too. Rotate overrides drag.
            if (S.I.IM.PC.WorldGameplay.DragCamera.IsPressed() &&
                !S.I.IM.PC.World.RotateCamera.IsPressed())
            {
                Ray ray = Camera.main.ScreenPointToRay(
                    S.I.IM.PC.World.MousePosition.ReadValue<Vector2>());
                RaycastHit hitData;
                // If you click on ground (as in not off screen/off the terrain), ...
                if (Physics.Raycast(ray, out hitData, 1000, _groundLayer))
                {
                    if (S.I.IM.PC.WorldGameplay.DragCamera.WasPressedThisFrame())
                    {
                        // Get the point on the ground where you originally clicked.
                        // Only happens the first frame you click.
                        _startDrag = ray.GetPoint(hitData.distance);
                    }
                    else
                    {
                        // Move camera leader the opposite direction you move mouse
                        transform.position += _startDrag - ray.GetPoint(hitData.distance);
                    }
                }
            }

            //--------------------------------------------------------

            // ROTATE CAMERA WHILE MOUSE WHEEL BUTTON HELD DOWN
            if (S.I.IM.PC.World.RotateCamera.IsPressed())
            {
                // Rotation around y-axis
                float deltaX =
                    S.I.IM.PC.World.MouseDelta.ReadValue<Vector2>().x *
                    _rotationSpeed;

                transform.RotateAround(_rotationOrigin.position, Vector3.up, deltaX);

                // TODO: Try using transform.rotation.GetAngleAxis 
                // Rotation around axis parallel to your local right vector, this axis always parallel to xz-plane.
                Vector3 axis = new Vector3(
                    -Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y),
                    0f,
                    Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y));

                float deltaY =
                    S.I.IM.PC.World.MouseDelta.ReadValue<Vector2>().y *
                    _rotationSpeed;

                // Clamp x-rotation between min and max values (at most 0 - 90).
                if (transform.rotation.eulerAngles.x - deltaY > _rotationXMin && transform.rotation.eulerAngles.x - deltaY <= _rotationXMax)
                {
                    transform.RotateAround(_rotationOrigin.position, axis, deltaY);
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
                    S.I.IM.PC.World.MousePosition.ReadValue<Vector2>();

                int mouseX = 0;
                int mouseY = 0;

                // Check if mouse screen position is near the edges
                if (mousePos.x > _screenWidth - _edgeDistance)
                {
                    mouseX = 1;
                }
                else if (mousePos.x < _edgeDistance)
                {
                    mouseX = -1;
                }

                if (mousePos.y > _screenHeight - _edgeDistance)
                {
                    mouseY = 1;
                }
                else if (mousePos.y < _edgeDistance)
                {
                    mouseY = -1;
                }

                // Direction we want to move
                Vector3 edgeScrollMovement = (_forward * mouseY) + (_right * mouseX);

                // Move camera
                transform.position += edgeScrollMovement * _edgeScrollingSpeed * Time.deltaTime;
            }
//#endif
        }
    }
}