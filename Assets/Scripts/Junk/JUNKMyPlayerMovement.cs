using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JUNKMyPlayerMovement : MonoBehaviour
{
    /*[SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float speed = 5f;
    private Vector3 input;
    private Matrix4x4 matrix = Matrix4x4.Rotate(Quaternion.Euler(0f, 45f, 0f));

    private void Update()
    {
        GatherInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GatherInput()
    {
        // Also use this vector to rotate the player on its y-axis.
        // If input's magnitude is less than like 0.1f, then have idle animation,
        // otherwise have running animation.
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        animator.SetFloat("Speed", input.sqrMagnitude);

        input.Normalize();
    }

    private void Move()
    {
        Vector3 skewedInput = matrix.MultiplyPoint3x4(input);

        // Only change facing direction while moving
        if (skewedInput.sqrMagnitude >= 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(skewedInput);
        }

        rb.MovePosition(transform.position + skewedInput * speed * Time.fixedDeltaTime);
    }*/
}