using UnityEngine;

public class GameManager : MonoBehaviour
{
    public HealthBar healthBar;
    public UIManager uiManager;

    private float currentHealth = 100f;
    private float maxHealth = 100f;

    void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
        uiManager.UpdateScore(0);
        uiManager.UpdateLevel(1);
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
