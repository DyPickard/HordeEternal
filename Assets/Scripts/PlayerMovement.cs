using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed = 1.25f; // Reduced from 2.5f for slower gameplay
    [SerializeField] private Animator animator; // Animator component of the player
    [SerializeField] private Tilemap collisionTilemap;

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
        if (movementInput.magnitude > 0)
        {
            animator.SetBool("isMoving", true);

            Vector2 newPosition = rb.position + movementInput * speed * Time.fixedDeltaTime;

            Vector3Int tilePosition = collisionTilemap.WorldToCell(newPosition);
            TileBase tile = collisionTilemap.GetTile(tilePosition);

            if (tile == null) // No collision tile
            {
                rb.MovePosition(newPosition);
            }
            else
            {
                // Tile has collision � block movement
                animator.SetBool("isMoving", false);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // Flip sprite
        if (movementInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movementInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }


    public void StartKnockback(Vector2 direction, float force, float duration)
    {
        StartCoroutine(Knockback(direction, force, duration));
    }
    private IEnumerator Knockback(Vector2 direction, float force, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            rb.MovePosition(rb.position + direction * force * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    // Returns the player's current movement direction (normalized)
    public Vector2 GetMovementDirection()
    {
        return movementInput.normalized;
    }
}



