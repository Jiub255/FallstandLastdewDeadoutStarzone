using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JUNKMouseFollow : MonoBehaviour
{
/*    private PlayerInput playerInput;

    private InputAction mousePositionAction;

    private Mouse currentMouse;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        mousePositionAction = playerInput.actions["MousePosition"];
        currentMouse = Mouse.current;
    }
    void Update()
    {
        Vector2 mouseScreenPosition = currentMouse.position.ReadValue();
        Vector3 pushedScreenPosition = new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 10f);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pushedScreenPosition);
        transform.position = worldPosition; 


        *//*        Vector2 screenPosition = mousePositionAction.ReadValue<Vector2>();
                Vector2 pushedScreenPosition = new Vector3(screenPosition.x, screenPosition.y, 10f);

                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pushedScreenPosition);

                transform.position = worldPosition;*//*
    }*/
}