using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DONT THINK THIS WILL WORK BECAUSE THE COLLISIONS NEVER GET DETECTED WITHOUT PHYSICS MOVEMENT

// Put this on main camera's transparency collider child
public class JUNKTransparentizer3 : MonoBehaviour
{
    /*private Transform cameraTransform;

    private Transform playerCharacterTransform;

   // public static event Action onEnteredCollider;

    private void Awake()
    {
        cameraTransform = transform.parent.transform;
    }

    private void OnEnable()
    {
        PlayerCharacterController.onChangedSelectedCharacter += ChangeCurrentCharacter;
    }

    private void OnDisable()
    {
        PlayerCharacterController.onChangedSelectedCharacter -= ChangeCurrentCharacter;
    }

    private void ChangeCurrentCharacter(Transform newCharacterTransform)
    {
        if (newCharacterTransform.CompareTag("MainCamera"))
        {
            playerCharacterTransform = null;
        }
        else
        {
            playerCharacterTransform = newCharacterTransform;
        }
    }

    private void FixedUpdate()
    {
        if (playerCharacterTransform)
        {
            // set length of transparency collider to reach the player
            float distance = Vector3.Distance(cameraTransform.position, playerCharacterTransform.position);
           
            transform.localScale = new Vector3(
                transform.localScale.x, transform.localScale.y, distance * 2);

           *//* transform.position = Vector3.Lerp(
                cameraTransform.position, playerCharacterTransform.position, distance / 2);*//*

            // rotate transparency collider to look at player character
            transform.LookAt(playerCharacterTransform.position);
        }
        else
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.rotation = Quaternion.identity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // make thing transparent. use an event?
        Debug.Log("entered collider");
    }

    private void OnTriggerExit(Collider other)
    {
        // make thing normal again
        Debug.Log("exited collider");
    }*/
}