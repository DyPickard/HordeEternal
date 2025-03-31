using UnityEngine;

public class DragonFireball : MonoBehaviour
{
    [SerializeField] private GameObject impactEffectPrefab;
    [SerializeField] private float lifetime = 3f;
    private int damage = 1;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage();

            if (impactEffectPrefab != null)
            {
                GameObject impact = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
                Destroy(impact, 0.5f);
            }

            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy") && !other.CompareTag("Projectile"))
        {
            if (impactEffectPrefab != null)
            {
                GameObject impact = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
                Destroy(impact, 0.5f);
            }

            Destroy(gameObject);
        }
    }
}