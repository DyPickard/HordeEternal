using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int expValue = 1;
    [SerializeField] private int health = 1;
    [SerializeField] private int damage = 1;
    private GameObject player;
    private Transform playerTransform;

    void Start()
    {
        // Find player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj;
            playerTransform = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Move toward player
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // rotate to face player
        Vector3 scale = transform.localScale;
        scale.x = (direction.x < 0) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enemy Triggered with: " + other.name);
        if (other.CompareTag("Player"))
        {
            // get player's rigidbody
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            // knockback player
            Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
            float knockbackForce = 5f;
            float knockbackDuration = 0.02f;
            player.GetComponent<PlayerMovement>().StartKnockback(knockbackDir, knockbackForce, knockbackDuration);
            // damage player
            GameManager.Instance.TakeDamage();
        }
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
        }
    }

    private void DestroyEnemy()
    {
        // destroy gameobject
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
