using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JUNKMouseFollowGround : MonoBehaviour
{
/*    [SerializeField]
    private LayerMask groundLayer;

    *//*    private PlayerInput playerInput;

        private InputAction mousePositionAction;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            mousePositionAction = playerInput.actions["MousePosition"];
        }*//*

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(
            MasterSingleton.Instance.InputManager.mousePositionAction.ReadValue<Vector2>());
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000, groundLayer))
        {
            transform.position = hitData.point;
        }
    }*/
}