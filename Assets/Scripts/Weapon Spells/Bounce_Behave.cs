using System.Net.NetworkInformation;
using UnityEngine;

public class Bounce_Behave : MonoBehaviour
{
    public float projectilespd;
    public GameObject impactef;
    private Rigidbody2D rb;

    public int bounces;
    public int maxBounces = 3;
    public int damage = 1;

    Enemy previoustarget = null;

    void Start()
    {
        Launch();
    }

    public void Launch()
    {
        rb = GetComponent<Rigidbody2D>();

        // Find the closest enemy
        Vector3 direction = GetTargetDirection();
        rb.linearVelocity = direction.normalized * projectilespd;

        // Rotate sprite to match velocity
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    Vector3 GetTargetDirection()
    {
        float closestDist = Mathf.Infinity;
        Enemy closest = null;

        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        foreach (Enemy e in allEnemies)
        {
            float dist = (e.transform.position - transform.position).sqrMagnitude;
            if (dist < closestDist && e != previoustarget)
            { 
                closestDist = dist;
                closest = e;
            }
        }
        previoustarget = closest;

        if (closest != null)
            return closest.transform.position - transform.position;

        // Default to forward if no enemies
        return Vector3.up;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("HoleInGround"))
        {
            Debug.Log("Bolt hit: " + other.name);
            GameObject explosion = Instantiate(impactef, transform.position, Quaternion.identity);
            Destroy(explosion, 0.1f);

            if (bounces >= maxBounces)
            {
                Destroy(gameObject);
            }
            else
            {
                bounces++;
                Launch();
            }

        }
    }
}
