using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// TODO - Add right click to drag camera. Base it off mouse screen position, not the ground. 
// Put this on Camera Focal Point
public class CameraMoveRotate : MonoBehaviour
{
    public static event Action OnCenterOnPC;

    [SerializeField, Range(0f, 10f), Header("Keyboard Movement")]
    private float _movementSpeed = 4f;

    private Transform _transform;
    private Vector3 _forward;
    private Vector3 _right;

    [SerializeField, Range(0f, 10f), Header("Rotation")]
    private float _xRotationSpeed = 1.5f;
    [SerializeField, Range(0f, 10f)]
    private float _yRotationSpeed = 1.5f;
    [SerializeField, Range(0f, 40f)]
    private float _xRotationMin = 7f;
    [SerializeField, Range(50f, 90f)]
    private float _xRotationMax = 90f;

    private const float _xRotationAbsoluteMin = 0.1f; 
    private const float _xRotationAbsoluteMax = 89.9f; 

    [SerializeField, Range(0f, 10f), Header("Edge Scrolling")]
    private float _edgeScrollingSpeed = 4f;
    [SerializeField, Range(0f, 30f), Tooltip("Edge scrolling zone thickness calculated using percentage of width")]
    private float _percentDistanceFromEdges = 10f;

    private float _screenWidth;
    private float _screenHeight;
    private float _edgeDistance;

    [SerializeField, Range(0f, 10f), Header("Camera Dragging")]
    // TODO - Tie drag speed to zoom level. 
    private float _draggingSpeed = 6f;

    private bool _dragging;
    private Vector2/*3*/ _lastMousePosition;
    private Camera _camera;

    // Input Actions
    private InputAction _zoomAction;
    private InputAction _rotateCameraAction;
    private InputAction _moveCameraAction;
    private InputAction _mouseDeltaAction;
    private InputAction _mousePositionAction;

    // Just doing this to make for better "speed" values in the inspector. 
    private float MovementSpeed { get { return _movementSpeed * 5f; } }
    private float VerticalRotationSpeed { get { return _xRotationSpeed / 10f; } }
    private float HorizontalRotationSpeed { get { return _yRotationSpeed / 10f; } }
    private float EdgeScrollingSpeed { get { return _edgeScrollingSpeed * 5f; } }
    private float DraggingSpeed { get { return _draggingSpeed / 2f; } }

    private void Start()
    { 
        _transform = transform;
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
        _edgeDistance = _screenWidth * (_percentDistanceFromEdges / 100);
        _camera = Camera.main;
        // Don't allow _xRotationMin to go to zero or lower, because it will mess up edge scrolling's xz-plane intersection calculations. 
        if (_xRotationMin < _xRotationAbsoluteMin) _xRotationMin = _xRotationAbsoluteMin;
        // Don't allow _xRotationMax to go to 90 or higher, because it makes the camera spin and looks bad. 
        // Might even set the absolute max a little lower than 89.9. 
        if (_xRotationMax > _xRotationAbsoluteMax) _xRotationMax = _xRotationAbsoluteMax;

        _zoomAction = S.I.IM.PC.Camera.Zoom;
        _rotateCameraAction = S.I.IM.PC.Camera.RotateCamera;
        _moveCameraAction = S.I.IM.PC.Camera.MoveCamera;
        _mouseDeltaAction = S.I.IM.PC.Camera.MouseDelta;
        _mousePositionAction = S.I.IM.PC.Camera.MousePosition;

        GetVectors();

       // _eventSystem = EventSystem.current;

        // "performed" is double click.
       // S.I.IM.PC.Home.SelectOrCenter.performed += CenterOnPC;

        PCSelector.OnDoubleClickPC += CenterOnPC;

        S.I.IM.PC.Camera.RightClick.started += StartDragging;
        S.I.IM.PC.Camera.RightClick.canceled += (c) => _dragging = false;
    }

    private void OnDisable()
    {
        //S.I.IM.PC.Home.SelectOrCenter.performed -= CenterOnPC;

        PCSelector.OnDoubleClickPC -= CenterOnPC;

        S.I.IM.PC.Camera.RightClick.started -= (c) => { _dragging = true; _lastMousePosition = _mousePositionAction.ReadValue<Vector2>(); }/*StartDragging*/;
        S.I.IM.PC.Camera.RightClick.canceled -= (c) => _dragging = false;
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

    private void LateUpdate()
    {
        // Zoom overrides everything else. Not noticeable since this action gets called only during
        // isolated frames, but it helps resolve some issues with moving while zooming.
        if (!_zoomAction.WasPerformedThisFrame())
        {
            // Can't drag and keyboard move at the same time, dragging overrides keyboard movement. 
            if (_dragging)
            {
                DragCamera();
            }
            else
            {
                KeyboardMove();
            }

            if (_rotateCameraAction.IsPressed())
            {
                GetVectors();
                RotateCamera();
            }
            // Don't edge scroll if holding down mouse wheel button.
            // Gets annoying when you accidentally edge scroll because you moved the mouse too far while rotating.
            else
            {
                // Turning off while making the game because it's annoying while in the editor. 
//                EdgeScroll();
            }
        }
    }

    // Called from double click of PC or PC UI button. 
    private void CenterOnPC(Transform pCTransform)
    {
        // TODO: Lerp quickly instead of instantly move there? Looks jumpy when you center on PC as is. 
        _transform.position = pCTransform.position;
        _transform.rotation = Quaternion.Euler(new Vector3(35f, 0f, 0f));
        // CameraZoom listens and sets zoom distance to default. 
        OnCenterOnPC?.Invoke();
    }

    // Only need to get these while rotating, because they don't change while moving or zooming. 
    // Also once when the script loads, so movement and zooming work from the beginning. 
    private void GetVectors()
    {
        _forward = _transform.forward;
        _right = _transform.right;

        // Project the forward and right vectors onto the horizontal plane (y = 0)
        _forward.y = 0f;
        _right.y = 0f;

        // Normalize them
        _forward.Normalize();
        _right.Normalize();
    }

    private void KeyboardMove()
    {
        // Get input
        Vector2 movement = _moveCameraAction.ReadValue<Vector2>();

        // Translate movement vector to world space
        Vector3 keyboardMovement = (_forward * movement.y) + (_right * movement.x);

        // Move
        _transform.position += keyboardMovement * MovementSpeed * Time.unscaledDeltaTime;
    }

    private void RotateCamera()
    {
        // Rotation around y-axis
        float deltaX =
            _mouseDeltaAction.ReadValue<Vector2>().x *
            HorizontalRotationSpeed;

        _transform.RotateAround(_transform.position, Vector3.up, deltaX);

        // Rotation around axis parallel to your local right vector, this axis always parallel to xz-plane.
        Vector3 axis = new Vector3(
            -Mathf.Cos(Mathf.Deg2Rad * _transform.rotation.eulerAngles.y),
            0f,
            Mathf.Sin(Mathf.Deg2Rad * _transform.rotation.eulerAngles.y));

        float deltaY =
            _mouseDeltaAction.ReadValue<Vector2>().y *
            VerticalRotationSpeed;

        // Clamp x-rotation between min and max values (at most 0 - 90).
        if (_transform.rotation.eulerAngles.x - deltaY > _xRotationMin && _transform.rotation.eulerAngles.x - deltaY <= _xRotationMax)
        {
            _transform.RotateAround(_transform.position, axis, deltaY);
        }
    }

    private void EdgeScroll()
    {
        // Get mouse screen position
        Vector3 mousePosition =
            _mousePositionAction.ReadValue<Vector2>();

        int mouseX = 0;
        int mouseY = 0;

        // Check if mouse screen position is near the edges
        if (mousePosition.x > _screenWidth - _edgeDistance)
        {
            mouseX = 1;
        }
        else if (mousePosition.x < _edgeDistance)
        {
            mouseX = -1;
        }

        if (mousePosition.y > _screenHeight - _edgeDistance)
        {
            mouseY = 1;
        }
        else if (mousePosition.y < _edgeDistance)
        {
            mouseY = -1;
        }

        // Direction we want to move
        Vector3 edgeScrollMovement = (_forward * mouseY) + (_right * mouseX);
        edgeScrollMovement.Normalize();

        // Move camera
        _transform.position += edgeScrollMovement * EdgeScrollingSpeed * Time.unscaledDeltaTime;
    }

    // Which way to do this? Just basic screen position based, constant speed drag for now and decide later. 

    // TODO - Either seriously limit or eliminate x-rotation, or get rid of drag movement. 
    // Doesn't work well when you can see far out into the distance, because your ground click can be really far away
    // so it'll move way too fast. 
    // Also, any change in x-rotation means a change in drag speed, shallower angles will have higher speeds. 
    // Could stick with rotation and do old screen position dragging too. Maybe incorporate zoom level and/or camera height
    // to change the drag speed so it doesn't feel way slower when you're zoomed out?

    // TODO - Calculate when rays cross xz-plane instead of raycasting to a giant plane. 

    // TODO - Tie drag speed to zoom level? Or camera height? 
    // OR, calculate drag amount based on ground distance instead of screen distance. 
    // Then it wont depend on zoom level. Do this instead. 
    // Just put a huge plane in the level on its own layer, and check distance using that so there's no interference or weird 
    // warping when you don't hit ground. 
    private void DragCamera()
    {
/*        Vector3 intersectionPoint;
        Ray ray = _camera.ScreenPointToRay(_mousePositionAction.ReadValue<Vector2>());

        if (ray.IntersectsXZPlane(out intersectionPoint))
        {
            // Get Movement Direction. 
            Vector3 movement = _lastMousePosition - intersectionPoint;

            // Move
            _transform.position += movement * _draggingSpeed * Time.unscaledDeltaTime;

            // Set last mouse position. 
            _lastMousePosition = intersectionPoint;
        }
        else
        {
            Debug.LogWarning($"No ground distance object found at screen point {_mousePositionAction.ReadValue<Vector2>()}. " +
                $"Something went wrong, the camera should always be pointing at the xz-plane. ");
        }


        RaycastHit[] hits = Physics.RaycastAll(
            _camera.ScreenPointToRay(_mousePositionAction.ReadValue<Vector2>()),
            // TODO - Set this to maximum zoom level + a little bit? 
            101f,
            _groundDistanceLayerMask);

        if (hits.Length == 1)
        {
            // Get Movement Direction. 
            Vector3 movement = _lastMousePosition - hits[0].point;

            // Move
            _transform.position += movement * _draggingSpeed * Time.unscaledDeltaTime;

            // Set last mouse position. 
            _lastMousePosition = hits[0].point;
        }
        else if (hits.Length > 1)
        {
            Debug.LogWarning($"{hits.Length} collisions with ground distance object detected. This should never happen, it's raycast toward a plane. ");
        }
        else
        {
            Debug.LogWarning($"No ground distance object found at screen point {_mousePositionAction.ReadValue<Vector2>()}");
        }*/

        // Get movement direction. 
        Vector2 direction = (_lastMousePosition - _mousePositionAction.ReadValue<Vector2>());

        // Translate direction vector to world space
        Vector3 movement = (_forward * direction.y) + (_right * direction.x);

        //        Debug.Log($"Mouse position: {_mousePositionAction.ReadValue<Vector2>()}, Last position: {_lastMousePosition}, movement: {movement}");

        // Move
        _transform.position += movement * _draggingSpeed * Time.unscaledDeltaTime;

        // Set last mouse position. 
        _lastMousePosition = _mousePositionAction.ReadValue<Vector2>();
    }

    private void StartDragging(InputAction.CallbackContext context)
    {
        _dragging = true;
        _lastMousePosition = _mousePositionAction.ReadValue<Vector2>();

/*        Vector3 intersectionPoint;
        Ray ray = _camera.ScreenPointToRay(_mousePositionAction.ReadValue<Vector2>());

        if (ray.IntersectsXZPlane(out intersectionPoint))
        {
            // Start dragging only if ground distance object hit. 
            _dragging = true;

            // Set last mouse position. 
            _lastMousePosition = intersectionPoint;
        }
        else
        {
            Debug.LogWarning($"No ground distance object found at screen point {_mousePositionAction.ReadValue<Vector2>()}. " +
                $"Something went wrong, the camera should always be pointing at the xz-plane. ");
        }


        RaycastHit[] hits = Physics.RaycastAll(
            _camera.ScreenPointToRay(_mousePositionAction.ReadValue<Vector2>()),
            101f,
            _groundDistanceLayerMask);

        if (hits.Length == 1)
        {
            // Start dragging only if ground distance object hit. 
            _dragging = true;

            // Set last mouse position. 
            _lastMousePosition = hits[0].point;
        }
        else if (hits.Length > 1)
        {
            Debug.LogWarning($"{hits.Length} collisions with ground distance object detected. This should never happen, it's raycast toward a plane. ");
        }
        else
        {
            Debug.LogWarning($"No ground distance object found at screen point {_mousePositionAction.ReadValue<Vector2>()}");
        }*/
    }
}