using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JUNKFollowCam : MonoBehaviour
{
/*    [SerializeField]
    private Transform followCamObject;

    private Camera mainCamera;

    // NEW INPUT SYSTEM STUFF
    private PlayerInput playerInput;

    private InputAction mousePositionAction;

    private float zoomDist = 10f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        mousePositionAction = playerInput.actions["MousePosition"];
        mainCamera = Camera.main;
    }
    private void Update()
    {
        Vector3 mousePos = mousePositionAction.ReadValue<Vector2>();
        Vector3 pushedMousePosition = new Vector3(mousePos.x, mousePos.y, zoomDist);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(pushedMousePosition);
        Vector3 groundedWorldPos = new Vector3(worldPos.x, 5f, worldPos.z);
        followCamObject.position = groundedWorldPos;
    }*/
}