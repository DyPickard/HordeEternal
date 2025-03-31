using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private int expValue = 1;
    [SerializeField] private int health = 1;
    [SerializeField] private int damage = 1;

    private GameObject player;
    private Transform playerTransform;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
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

    void OnTriggerEnter2D(Collider2D other)
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

    private void DestroyEnemy()
    {
        if (DropTableManager.Instance != null)
        {
            DropTableManager.Instance.HandleEnemyDeath(transform.position);
        }

        Destroy(gameObject);
        player.GetComponent<PlayerLevel>().IncreaseExp(expValue);
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DestroyEnemy();
        }
    }
}
