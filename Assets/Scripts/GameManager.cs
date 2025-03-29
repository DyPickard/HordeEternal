using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public AudioClip backgroundMusic;

    private const int maxLives = 6;
    private int currentLives = 3;

    void Start()
    {

        // uiManager.UpdateLives(currentLives);
        // uiManager.UpdateScore(0);
        // uiManager.ClearPowerUps();

        AudioManager.Instance.PlayMusic(backgroundMusic);

    }

    void GetLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            uiManager.UpdateLives(currentLives);
        }
    }

    void TakeDamage()
    {
        currentLives--;
        uiManager.UpdateLives(currentLives);
        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    void AddScore(int points)
    {
        uiManager.UpdateScore(points);
    }

    void PickUpWeapon(Sprite weaponSprite)
    {
        uiManager.SetWeaponPowerUp(weaponSprite);
    }

    void PickUpUtility(Sprite utilitySprite)
    {
        uiManager.SetUtilityPowerUp(utilitySprite);
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
    }
}
