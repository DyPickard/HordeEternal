using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Speed of the player
    [SerializeField] private Animator animator; // Animator component of the player
    private Vector2 movementInput;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        // Check if player is moving
        if (movementInput.magnitude > 0) // Fixed movement check
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // Flip character sprite based on movement direction
        if (movementInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movementInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Move the player
        rb.linearVelocity = movementInput * speed; // Fixed Rigidbody2D movement
    }
}



