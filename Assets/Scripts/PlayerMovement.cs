using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Speed of the player
    private Vector2 movementInput;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMove(InputValue value)
    {
        // debug message
        Debug.Log("OnMove called");
        movementInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector2 movement = movementInput * speed;
        rb.linearVelocity = movement;
    }
}


