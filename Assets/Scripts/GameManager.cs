using UnityEngine;

public class GameManager : MonoBehaviour
{
<<<<<<< Updated upstream
    public UIManager uiManager;
=======
    private UIManager uiManager;
    public AudioClip backgroundMusic;

>>>>>>> Stashed changes
    private const int maxLives = 6;
    [SerializeField] private int currentLives = 3;
    private int currentScore = 0;


    void Start()
    {
<<<<<<< Updated upstream
        uiManager.UpdateLives(currentLives);
        uiManager.UpdateScore(0);
        uiManager.ClearPowerUps();
=======

        uiManager = FindObjectOfType<UIManager>();

        if (uiManager == null)
        {
            Debug.LogError("UIManager not found! Make sure UIManager is loaded from the Preload scene.");
            return;
        }

        uiManager.UpdateLives(currentLives);
        uiManager.UpdateScore(currentScore);
        uiManager.ClearPowerUps();

        if (backgroundMusic != null)
        {
            AudioManager.Instance.PlayMusic(backgroundMusic);
        }
>>>>>>> Stashed changes
    }

    void OnValidate()
    {
        if (uiManager != null)
        {
            SetLives(currentLives);
        }
    }

    public void SetLives(int newLives)
    {
        currentLives = Mathf.Clamp(newLives, 0, maxLives);
        uiManager.UpdateLives(currentLives);
    }

    public void GetLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            uiManager.UpdateLives(currentLives);
        }
    }

    public void TakeDamage()
    {
        currentLives--;
        uiManager.UpdateLives(currentLives);
        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
        uiManager.UpdateScore(currentScore);
    }

    public void PickUpWeapon(Sprite weaponSprite)
    {
        uiManager.SetWeaponPowerUp(weaponSprite);
    }

    public void PickUpUtility(Sprite utilitySprite)
    {
        uiManager.SetUtilityPowerUp(utilitySprite);
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
    }
}