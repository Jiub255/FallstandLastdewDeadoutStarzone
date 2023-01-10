using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

// Putting this on the camera for now. Makes sense
public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField]
    private LayerMask transparentable;

    private NavMeshAgent currentAgent = null;

    private CharacterAnimation playerCharacterAnimation = null;

    public static event Action<Transform> onChangedSelectedCharacter;

    private void Start()
    {
        S.I.InputManager.playerControls.World.Select.performed += LeftClick;
        S.I.InputManager.playerControls.World.DragCamera.performed += RightClick;
    }

    private void OnDisable()
    {
        S.I.InputManager.playerControls.World.Select.performed -= LeftClick;
        S.I.InputManager.playerControls.World.DragCamera.performed -= RightClick;
    }

    private void LeftClick(InputAction.CallbackContext context)
    {
        RaycastHit hit;

        // ~transparentable makes the raycast ignore objects in Transparentable layer,
        //      so you can click on the other side of walls
        if (Physics.Raycast(Camera.main.ScreenPointToRay(
                S.I.InputManager.playerControls.World.MousePosition.ReadValue<Vector2>()), 
                out hit, 
                100, 
                ~transparentable))
        {
            // TODO: Check for certain class/interface that all PC's will have instead of tag
            //      NavMeshAgent wont work since enemies will probably use them too
            // Select new currentAgent if clicking on a PC
            if (hit.collider.CompareTag("PlayerCharacter"))
            {
                NavMeshAgent agent = hit.transform.GetComponent<NavMeshAgent>();

                if (agent != null)
                {
                    // deactivate old currentAgent's selected icon
                    currentAgent?.transform.GetChild(1).gameObject.SetActive(false);
                    // set new character
                    currentAgent = agent;
                    onChangedSelectedCharacter?.Invoke(currentAgent.transform);
                    // activate new currentAgent's selected icon
                    currentAgent.transform.GetChild(1).gameObject.SetActive(true);
                    // get reference to new currentAgent's animation script
                    playerCharacterAnimation = currentAgent.GetComponent<CharacterAnimation>();
                }
            }
            // move currentAgent (if there is one) to click position otherwise
            else if (currentAgent != null)
            {
                currentAgent.destination = hit.point;
                playerCharacterAnimation.isMoving = true;
            }
        }
    }

    private void RightClick(InputAction.CallbackContext context)
    {
        if (currentAgent != null)
        {
            // deactivate old currentAgent's selected icon
            currentAgent.transform.GetChild(1).gameObject.SetActive(false);

            playerCharacterAnimation = null;
            currentAgent = null;
            onChangedSelectedCharacter.Invoke(transform);
        }
    }
}