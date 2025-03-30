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

    // Movement ability properties
    [SerializeField] private float movementAbilityCooldown = 3f;
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashDuration = 0.2f;
    private bool movementAbilityReady = true;
    private float movementAbilityCooldownTimer = 0f;
    private PlayerMovement playerMovement;

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
        playerMovement = FindObjectOfType<PlayerMovement>();

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

    void Update()
    {
        // Check for movement ability input
        if (Input.GetKeyDown(KeyCode.Space) && movementAbilityReady)
        {
            UseMovementAbility();
        }

        if (!movementAbilityReady)
        {
            movementAbilityCooldownTimer -= Time.deltaTime;
            if (movementAbilityCooldownTimer <= 0)
            {
                movementAbilityReady = true;
                Debug.Log("Movement ability ready!");
            }
        }
    }

    private void UseMovementAbility()
    {
        Debug.Log("Movement ability used!");

        if (playerMovement != null)
        {
            Vector2 dashDirection = playerMovement.GetMovementDirection();

            // If player isn't actively moving, dash forward based on facing direction
            if (dashDirection.magnitude < 0.1f)
            {
                float facingX = player.transform.localScale.x;
                dashDirection = new Vector2(facingX, 0).normalized;
            }

            if (dashDirection.magnitude > 0)
            {
                StartCoroutine(PerformDash(dashDirection));
            }
            else
            {
                Debug.Log("No valid dash direction found!");
            }
        }
        else
        {
            Debug.LogError("PlayerMovement component not found!");
        }

        movementAbilityReady = false;
        movementAbilityCooldownTimer = movementAbilityCooldown;
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        Debug.Log("Dashing in direction: " + direction);

        float startTime = Time.time;
        Vector2 startPosition = player.transform.position;
        Vector2 targetPosition = startPosition + direction * dashDistance;

        // Temporarily disable player control during dash
        bool wasControlEnabled = playerMovement.enabled;
        playerMovement.enabled = false;

        // Make player temporarily invulnerable during dash
        bool wasInvulnerable = isInvulnerable;
        isInvulnerable = true;

        while (Time.time < startTime + dashDuration)
        {
            float t = (Time.time - startTime) / dashDuration;
            player.transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        player.transform.position = targetPosition;

        playerMovement.enabled = wasControlEnabled;

        isInvulnerable = wasInvulnerable;

        Debug.Log("Dash completed!");
    }

}
