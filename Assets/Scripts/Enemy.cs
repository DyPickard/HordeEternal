using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private Transform player;

    void Start()
    {
        // Find player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Move toward player
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // rotate to face player
        Vector3 scale = transform.localScale;
        scale.x = (direction.x < 0) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with: " + other.name);
        if (other.CompareTag("Player"))
        {
            // Damage logic here
            Destroy(gameObject);
        }
    }

}
