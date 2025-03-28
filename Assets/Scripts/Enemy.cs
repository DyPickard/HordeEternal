using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int expValue = 1;
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
            // Damage logic here
            EnemyKilled();
            // call IncreaseExp() on PlayerLevel script
            other.GetComponent<PlayerLevel>().IncreaseExp(expValue); 
            Debug.Log("Player gained " + expValue + " exp");
        }
        if (other.CompareTag("Bullet"))
        {
            // take damage
        }
    }

    private void EnemyKilled()
    {
        // destroy gameobject
        Destroy(gameObject);
    }

}
