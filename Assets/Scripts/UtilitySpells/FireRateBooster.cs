using UnityEngine;
using System.Collections;

public class FireRateBooster : UtilitySpell
{
    [SerializeField] private float fireRateMultiplier = 2f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private Color glowColor = new Color(1f, 1f, 0f, 0.5f); // Yellow glow
    private bool isActive = false;
    private Fire_Bolt fireBolt;
    private SpriteRenderer playerSprite;
    private Color originalColor;

    private void Start()
    {
        FindWeaponComponent();
        if (GameManager.Instance?.Player != null)
        {
            playerSprite = GameManager.Instance.Player.GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                originalColor = playerSprite.color;
            }
        }
    }

    private void FindWeaponComponent()
    {
        fireBolt = FindWeaponSpellComponent<Fire_Bolt>();
    }

    public override void Activate()
    {
        Debug.Log("FireRateBooster Activate() called");
        if (!isActive)
        {
            if (fireBolt == null)
            {
                FindWeaponComponent();
            }

            if (fireBolt != null)
            {
                Debug.Log("Starting FireRateBooster effect");
                StartCoroutine(BoostFireRate());
            }
            else
            {
                Debug.LogError("Cannot activate FireRateBooster - Fire_Bolt component not found!");
            }
        }
        else
        {
            Debug.Log("FireRateBooster is already active!");
        }
    }

    private IEnumerator BoostFireRate()
    {
        isActive = true;

        int originalFireRate = fireBolt.baserate;
        int boostedFireRate = Mathf.RoundToInt(originalFireRate / fireRateMultiplier);
        Debug.Log($"Boosting fire rate: Reducing delay between shots from {originalFireRate} to {boostedFireRate} frames");
        fireBolt.baserate = boostedFireRate;

        if (playerSprite != null)
        {
            playerSprite.color = glowColor;
        }

        Debug.Log($"FireRateBooster active for {duration} seconds");
        yield return new WaitForSeconds(duration);

        Debug.Log($"Restoring fire rate back to {originalFireRate}");
        fireBolt.baserate = originalFireRate;
        if (playerSprite != null)
        {
            playerSprite.color = originalColor;
        }
        isActive = false;

        Debug.Log("FireRateBooster effect complete, clearing from inventory");
        PlayerSpellManager spellManager = GameManager.Instance.Player.GetComponent<PlayerSpellManager>();
        if (spellManager != null)
        {
            spellManager.ClearUtilitySpell();
        }
    }

    private void OnDestroy()
    {
        if (isActive)
        {
            if (fireBolt != null)
            {
                fireBolt.baserate = fireBolt.baserate * 2; // Restore to original rate
            }
            if (playerSprite != null)
            {
                playerSprite.color = originalColor;
            }
        }
    }
}