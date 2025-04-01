using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(FlashOnHit))]
public class Enemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected int expValue = 1;
    [SerializeField] protected int health = 1;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float deathAnimationDuration = 0.5f;

    protected GameObject player;
    protected Transform playerTransform;
    protected Rigidbody2D rb;
    protected FlashOnHit flashEffect;
    protected Animator animator;
    protected bool isDying = false;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        rb = GetComponent<Rigidbody2D>();
        flashEffect = GetComponent<FlashOnHit>();
        animator = GetComponent<Animator>();
    }

    protected virtual void FixedUpdate()
    {
        if (player == null || isDying) return;

        // Move toward player using physics
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Vector2 newPosition = rb.position + (Vector2)(direction * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        // Rotate to face player
        Vector3 scale = transform.localScale;
        scale.x = (direction.x < 0) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (isDying) return;

        Debug.Log("Enemy Triggered with: " + other.name);
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
            float knockbackForce = 5f;
            float knockbackDuration = 0.02f;
            player.GetComponent<PlayerMovement>().StartKnockback(knockbackDir, knockbackForce, knockbackDuration);
            GameManager.Instance.TakeDamage();
        }

        if (other.CompareTag("Bullet"))
        {
            var bolt = other.GetComponent<Bolt_Behavior>();
            if (bolt != null)
            {
                TakeDamage(bolt.damage);
            }
            else
            {
                var bounce = other.GetComponent<Bounce_Behave>();
                if (bounce != null)
                {
                    TakeDamage(bounce.damage);
                }
                else
                {
                    TakeDamage(1); // fallback
                }
            }
        }
    }

    protected virtual void DestroyEnemy()
    {
        if (!isDying)
        {
            StartCoroutine(DeathSequence());
        }
    }

    protected virtual IEnumerator DeathSequence()
    {
        isDying = true;
        Debug.Log($"Starting death sequence for {gameObject.name}");

        // Disable all colliders
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        if (rb != null)
        {
            rb.simulated = false;
        }

        if (animator != null)
        {
            animator.SetBool("isDead", true);
            Debug.Log($"Set isDead to true on {gameObject.name}");

            yield return new WaitForSeconds(0.1f);

            bool isInKilledState = animator.GetCurrentAnimatorStateInfo(0).IsName("Killed");
            Debug.Log($"Is in Killed state: {isInKilledState}");

            yield return new WaitForSeconds(deathAnimationDuration);
        }

        Debug.Log($"Death sequence complete for {gameObject.name}, destroying object");

        if (player != null)
        {
            player.GetComponent<PlayerLevel>().IncreaseExp(expValue);
        }

        if (DropTableManager.Instance != null)
        {
            DropTableManager.Instance.HandleEnemyDeath(transform.position);
        }

        Destroy(gameObject);
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDying) return;

        health -= damage;
        if (flashEffect != null)
        {
            flashEffect.Flash();
        }

        if (health <= 0)
        {
            DestroyEnemy();
        }
    }
}
