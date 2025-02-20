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

    private void Start()
    {
        // Get reference to the Move action from Project Settings
        var moveAction = InputActionReference.Create(InputSystem.ListEnabledActions()[0]);

        // Assign event listeners
        moveAction.action.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        moveAction.action.canceled += ctx => movementInput = Vector2.zero;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movementInput * speed;
    }
}
