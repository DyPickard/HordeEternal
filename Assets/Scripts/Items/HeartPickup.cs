using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public AudioClip pickupSound;

    void Start()
    {
        pickupSound = Resources.Load<AudioClip>("Spells/GainLife");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GetLife();
                AudioManager.Instance.PlaySFX(pickupSound);
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