using UnityEngine;

public class GameManager : MonoBehaviour
{
    public HealthBar healthBar;
    private float currentHealth = 100f;
    private float maxHealth = 100f;

    void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
    }

    void takeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
