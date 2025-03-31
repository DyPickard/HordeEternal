using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GetLife();


                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * 2f) * 0.1f;
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + yOffset * Time.deltaTime,
            transform.position.z
        );
    }
}