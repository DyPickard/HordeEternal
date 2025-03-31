using UnityEngine;
using System.Collections;

public class GhostEnemy : Enemy
{
    private Vector2 previousPosition;
    private const float GHOST_ALPHA = 0.7f;
    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();

        moveSpeed = 1.5f;
        health = 2;
        expValue = 2;

        previousPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();

        SetGhostTransparency();

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

        if (flashEffect != null)
        {
            flashEffect.OnFlashComplete += SetGhostTransparency;
        }
    }

    private void OnDestroy()
    {
        if (flashEffect != null)
        {
            flashEffect.OnFlashComplete -= SetGhostTransparency;
        }
    }

    private void SetGhostTransparency()
    {
        if (spriteRenderer != null)
        {
            Color ghostColor = spriteRenderer.color;
            ghostColor.a = GHOST_ALPHA;
            spriteRenderer.color = ghostColor;
        }
    }

    protected override void FixedUpdate()
    {
        if (player == null || isDying) return;

        previousPosition = rb.position;

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        rb.MovePosition(rb.position + (Vector2)(direction * moveSpeed * Time.fixedDeltaTime));

        Vector3 scale = transform.localScale;
        scale.x = (direction.x < 0) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;

        if (animator != null)
        {
            float speed = ((Vector2)transform.position - previousPosition).magnitude / Time.fixedDeltaTime;
            animator.SetBool("isWalking", speed > 0.01f);
        }
    }

    protected override IEnumerator DeathSequence()
    {
        yield return StartCoroutine(base.DeathSequence());
        SetGhostTransparency();
    }
}