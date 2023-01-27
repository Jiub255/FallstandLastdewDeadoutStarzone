using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Put this on Rotation Origin
public class CameraMoveRotate : MonoBehaviour
{
    public static event Action OnCenterOnPC;

    [SerializeField, Range(0f, 50f), Header("Keyboard Movement")]
    private float _movementSpeed = 20f;

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

    [SerializeField, Range(0f, 30f), Tooltip("Calculated using percent of width")]
    private float _percentDistanceFromEdges = 10f;

    private float _screenWidth;
    private float _screenHeight;
    private float _edgeDistance;

    [/*SerializeField,*/ Header("For Centering on PC")]
    //private SelectedPCSO _selectedPCSO;

    [SerializeField]
    private LayerMask _pCLayer;

    private void Start()
    {
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
        _edgeDistance = _screenWidth * (_percentDistanceFromEdges / 100);

        // "performed" is double click.
        S.I.IM.PC.Home.SelectOrCenter.performed += CenterOnPC;

        PCSelector.OnDoubleClickPCButton += CenterOnPC;
    }

    private void OnDisable()
    {
        S.I.IM.PC.Home.SelectOrCenter.performed -= CenterOnPC;

        PCSelector.OnDoubleClickPCButton -= CenterOnPC;
    }

    private void LateUpdate()
    {
        // Zoom overrides everything else. Not noticeable since this action gets called only during
        // isolated frames, but it helps resolve some issues with moving while zooming.
        if (!S.I.IM.PC.World.Zoom.WasPerformedThisFrame())
        {
            GetVectors();

            KeyboardMove();

            if (S.I.IM.PC.World.RotateCamera.IsPressed())
            {
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

    private void CenterOnPC(InputAction.CallbackContext context)
    {
        // Only raycast to PC layer. 

        RaycastHit[] hits = Physics.RaycastAll(
            Camera.main.ScreenPointToRay(S.I.IM.PC.World.MousePosition.ReadValue<Vector2>()),
            1000,
            _pCLayer);

        if (hits.Length > 0)
        {
            // If double clicked on PC, center camera on them.
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("PlayerCharacter")) && 
                    !EventSystem.current.IsPointerOverGameObject())
                {
                    CenterOnPC(hit.collider.transform);
                    // "return" so that you center on first one raycast hits, in case another PC is behind them. 
                    return;
                }
            }
        }
    }

    // Want to call this from double click, or at least just single click, of PC UI button. 
    private void CenterOnPC(Transform pCTransform)
    {
        // Maybe lerp quickly instead of instantly move there? Looks jumpy when you center on PC as is.
        transform.position = pCTransform.position;
        transform.rotation = Quaternion.Euler(new Vector3(35f, 0f, 0f));
        // CameraZoom listens and sets zoom distance to default. 
        OnCenterOnPC?.Invoke();
    }

    private void GetVectors()
    {
        _forward = transform.forward;
        _right = transform.right;

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
        Vector2 movement = S.I.IM.PC.World.MoveCamera.ReadValue<Vector2>();

        // Translate movement vector to world space
        Vector3 keyboardMovement = (_forward * movement.y) + (_right * movement.x);

        // Move
        transform.position += keyboardMovement * _movementSpeed * Time.unscaledDeltaTime;
    }

    private void RotateCamera()
    {
        // Rotation around y-axis
        float deltaX =
            S.I.IM.PC.World.MouseDelta.ReadValue<Vector2>().x *
            _rotationSpeed;

        transform.RotateAround(transform.position, Vector3/*transform*/.up, deltaX);

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
            transform.RotateAround(transform.position, axis, deltaY);
        }
    }

    private void EdgeScroll()
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
        edgeScrollMovement.Normalize();

        // Move camera
        transform.position += edgeScrollMovement * _edgeScrollingSpeed * Time.unscaledDeltaTime;
    }
}