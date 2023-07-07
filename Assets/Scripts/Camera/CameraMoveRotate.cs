using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// TODO - Add right click to drag camera. Base it off mouse screen position, not the ground. 
// Put this on Camera Focal Point
public class CameraMoveRotate : MonoBehaviour
{
    public static event Action OnCenterOnPC;

    [SerializeField, Range(0f, 50f), Header("Keyboard Movement")]
    private float _movementSpeed = 20f;

    private Transform _transform;
    private Vector3 _forward;
    private Vector3 _right;

    [SerializeField, Range(0f, 2f), Header("Rotate")]
    private float _rotationSpeed = 0.15f;

    [SerializeField, Range(0f, 40f)]
    private float _rotationXMin = 7f;

    [SerializeField, Range(50f, 90f)]
    private float _rotationXMax = 90f;

    [SerializeField, Range(0f, 50f), Header("Edge Scrolling")]
    private float _edgeScrollingSpeed = 20f;

    [SerializeField, Range(0f, 30f), Tooltip("Edge scrolling zone thickness calculated using percentage of width")]
    private float _percentDistanceFromEdges = 10f;

    private float _screenWidth;
    private float _screenHeight;
    private float _edgeDistance;

    [SerializeField, Range(0f, 3f), Header("Drag Camera")]
    private float _draggingSpeed = 2f;
    private bool _dragging;
    private Vector2 _lastMousePosition;

    // Input Actions
    private InputAction _zoomAction;
    private InputAction _rotateCameraAction;
    private InputAction _moveCameraAction;
    private InputAction _mouseDeltaAction;
    private InputAction _mousePositionAction;

    private void Start()
    { 
        _transform = transform;

        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
        _edgeDistance = _screenWidth * (_percentDistanceFromEdges / 100);

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

        S.I.IM.PC.Camera.RightClick.started += (c) => { _dragging = true; _lastMousePosition = _mousePositionAction.ReadValue<Vector2>(); };
        S.I.IM.PC.Camera.RightClick.canceled += (c) => _dragging = false;
    }

    private void OnDisable()
    {
        //S.I.IM.PC.Home.SelectOrCenter.performed -= CenterOnPC;

        PCSelector.OnDoubleClickPC -= CenterOnPC;

        S.I.IM.PC.Camera.RightClick.started -= (c) => { _dragging = true; _lastMousePosition = _mousePositionAction.ReadValue<Vector2>(); };
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
               // EdgeScroll();
            }
        }
    }

    private void DragCamera()
    {
        // Get movement direction. 
        Vector2 direction = (_lastMousePosition - _mousePositionAction.ReadValue<Vector2>());

        // Translate direction vector to world space
        Vector3 movement = (_forward * direction.y) + (_right * direction.x);

        Debug.Log($"Mouse position: {_mousePositionAction.ReadValue<Vector2>()}, Last position: {_lastMousePosition}, movement: {movement}");

        // Move
        _transform.position += movement * _draggingSpeed * Time.unscaledDeltaTime;

        // Set last mouse position. 
        _lastMousePosition = _mousePositionAction.ReadValue<Vector2>();
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
        _transform.position += keyboardMovement * _movementSpeed * Time.unscaledDeltaTime;
    }

    private void RotateCamera()
    {
        // Rotation around y-axis
        float deltaX =
            _mouseDeltaAction.ReadValue<Vector2>().x *
            _rotationSpeed;

        _transform.RotateAround(_transform.position, Vector3.up, deltaX);

        // Rotation around axis parallel to your local right vector, this axis always parallel to xz-plane.
        Vector3 axis = new Vector3(
            -Mathf.Cos(Mathf.Deg2Rad * _transform.rotation.eulerAngles.y),
            0f,
            Mathf.Sin(Mathf.Deg2Rad * _transform.rotation.eulerAngles.y));

        float deltaY =
            _mouseDeltaAction.ReadValue<Vector2>().y *
            _rotationSpeed;

        // Clamp x-rotation between min and max values (at most 0 - 90).
        if (_transform.rotation.eulerAngles.x - deltaY > _rotationXMin && _transform.rotation.eulerAngles.x - deltaY <= _rotationXMax)
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
        _transform.position += edgeScrollMovement * _edgeScrollingSpeed * Time.unscaledDeltaTime;
    }
}