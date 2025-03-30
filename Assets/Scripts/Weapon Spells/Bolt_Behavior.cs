using UnityEngine;

public class Bolt_Behavior : MonoBehaviour
{
    public float projectilespd;
    public GameObject impactef;
    private new Rigidbody2D rigidbody;
    void Start()
    {
        float distance = Mathf.Infinity;
        Enemy closest = null;
        Enemy[] allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        
        foreach (Enemy currentE in allEnemies)
        {
            float distanceIteration = (currentE.transform.position - transform.position).sqrMagnitude;
            if (distanceIteration < distance)
            {
                distance = distanceIteration;
                closest = currentE;
            }
        }

        Vector3 aim = closest.transform.position - transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.linearVelocity = new Vector2(aim.x, aim.y).normalized * projectilespd;

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") == false)
        {
            Debug.Log("Bolt hit: " + other.name);
            //Instantiate(impactef, transform.position, Quaternion.identity);
            GameObject explosion = Instantiate(impactef, transform.position, Quaternion.identity);
            Destroy(explosion, 0.1f); // Adjust duration to explosion length
            Destroy(gameObject);
        }
    }
}
