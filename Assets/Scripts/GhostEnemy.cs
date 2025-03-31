using UnityEngine;

public class GhostEnemy : Enemy
{
    private Animator animator;
    private Vector2 previousPosition;

    protected void Start()
    {
        base.Start();

        moveSpeed = 1.5f;
        health = 2;
        expValue = 2;

        animator = GetComponent<Animator>();
        previousPosition = transform.position;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color ghostColor = spriteRenderer.color;
            ghostColor.a = 0.7f;
            spriteRenderer.color = ghostColor;
        }

        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.excludeLayers = LayerMask.GetMask("Ground", "Collision");

            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.isTrigger = true;
            }
        }
    }

    protected override void FixedUpdate()
    {
        if (player == null) return;

        previousPosition = rb.position;

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        rb.MovePosition(rb.position + (Vector2)(direction * moveSpeed * Time.fixedDeltaTime));

        Vector3 scale = transform.localScale;
        scale.x = (direction.x < 0) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;

        if (animator != null)
        {
            float speed = ((Vector2)transform.position - previousPosition).magnitude / Time.fixedDeltaTime;
            animator.SetFloat("Speed", speed);
        }
    }
}