using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public UIManager uiManager;
    public AudioClip backgroundMusic;

    private const int maxLives = 6;
    [SerializeField] private int currentLives = 3;

    // singleton pattern
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // prevent duplicates
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // optional
    }


    // singleton pattern
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // prevent duplicates
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // optional
    }


    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        uiManager.UpdateLives(currentLives);
        uiManager.UpdateScore(0);
        uiManager.ClearPowerUps();

        if (backgroundMusic != null)
        {
            AudioManager.Instance.PlayMusic(backgroundMusic);
        }
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

    void GetLife()
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
        //uiManager.UpdateLives(currentLives);
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
