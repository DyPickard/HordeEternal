using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(FlashOnHit))]
public class Enemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected int expValue = 1;
    [SerializeField] protected int health = 1;
    [SerializeField] protected int damage = 1;

    protected GameObject player;
    protected Transform playerTransform;
    protected Rigidbody2D rb;
    protected FlashOnHit flashEffect;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        rb = GetComponent<Rigidbody2D>();
        flashEffect = GetComponent<FlashOnHit>();
    }

    protected virtual void FixedUpdate()
    {
        if (player == null) return;

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
            TakeDamage(1);
        }
    }

    protected virtual void DestroyEnemy()
    {
        Destroy(gameObject);
        player.GetComponent<PlayerLevel>().IncreaseExp(expValue);
    }

    protected virtual void TakeDamage(int damage)
    {
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
