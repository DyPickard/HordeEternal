using UnityEngine;
using System.Collections;

public class DragonEnemy : Enemy
{
    [Header("Dragon Settings")]
    [SerializeField] private float fireballCooldown = 2f;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireballSpeed = 5f;

    private float lastFireballTime;

    protected override void Start()
    {
        base.Start();

        moveSpeed = 1.5f;
        health = 15;
        expValue = 10;
        damage = 2;

        lastFireballTime = -fireballCooldown; // Allow immediate fireball
    }

    protected override void FixedUpdate()
    {
        if (player == null || isDying) return;

        base.FixedUpdate();

        if (Time.time >= lastFireballTime + fireballCooldown)
        {
            ShootFireball();
        }
    }

    private void ShootFireball()
    {
        if (fireballPrefab != null)
        {
            lastFireballTime = Time.time;
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

            Rigidbody2D fireballRb = fireball.GetComponent<Rigidbody2D>();
            if (fireballRb != null)
            {
                fireballRb.linearVelocity = direction * fireballSpeed;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                fireball.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (isDying) return;

        if (other.CompareTag("Player"))
        {
            Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
            float knockbackForce = 5f;
            float knockbackDuration = 0.2f;
            player.GetComponent<PlayerMovement>().StartKnockback(knockbackDir, knockbackForce, knockbackDuration);
            GameManager.Instance.TakeDamage();
        }

        base.OnTriggerEnter2D(other);
    }
}