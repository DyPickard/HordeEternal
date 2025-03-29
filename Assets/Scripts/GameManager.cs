using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public UIManager uiManager;
    public AudioClip backgroundMusic;
    public AudioClip gameOverMusic;

    public PlayerLevel playerLevel;
    private const int maxLives = 6;
    [SerializeField] private int currentLives = 3;
    [SerializeField] private GameObject player;

    private Coroutine flashCoroutine;
    private Color originalColor;
    private SpriteRenderer playerSpriteRenderer;

    private bool isInvulnerable = false;



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
        playerLevel = FindObjectOfType<PlayerLevel>();

        if (playerLevel != null)
        {
            playerLevel.InitializeUI(uiManager);
            if (player != null)
            {
                playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
                if (playerSpriteRenderer != null)
                {
                    originalColor = playerSpriteRenderer.color;
                }
                else
                {
                    Debug.LogError("SpriteRenderer not found on player!");
                }
            }
            else
            {
                Debug.LogError("Player not found!");
            }

            uiManager.UpdateLives(currentLives);
            uiManager.ClearPowerUps();

            {
                AudioManager.Instance.PlayMusic(backgroundMusic);
            }
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
        if (isInvulnerable)
            return;

        Debug.Log("Player took damage!");
        currentLives--;
        FlashSpriteRed();
        uiManager.UpdateLives(currentLives);

        if (currentLives <= 0)
        {
            GameOver();
        }

        StartCoroutine(InvulnerabilityCoroutine());
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(1f);
        isInvulnerable = false;
    }


    public void AddExperience(int expAmount)
    {
        playerLevel.IncreaseExp(expAmount);
    }

    public void SetLevel(int newLevel)
    {
        playerLevel.Level = newLevel;
    }

    public void SetExperience(int newExp)
    {
        playerLevel.Exp = newExp;
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
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic(gameOverMusic);
        uiManager.GameOver();
    }

    void FlashSpriteRed()
    {
        Debug.Log("Flash coroutine called");
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine); 

        flashCoroutine = StartCoroutine(FlashSpriteRedCoroutine());
    }

    private IEnumerator FlashSpriteRedCoroutine()
    {
        Debug.Log("Flash coroutine started");
        if (playerSpriteRenderer == null)
            yield break;

        Color flashColor = Color.red;
        float flashDuration = 0.1f;
        int flashCount = 3;

        for (int i = 0; i < flashCount; i++)
        {
            playerSpriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            playerSpriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }

        // Ensure color is fully reset after all flashing
        playerSpriteRenderer.color = originalColor;
        flashCoroutine = null;
    }



}
