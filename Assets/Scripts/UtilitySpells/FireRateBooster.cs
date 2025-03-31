using UnityEngine;
using System.Collections;

public class FireRateBooster : UtilitySpell
{
    [SerializeField] private float fireRateMultiplier = 2f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private Color glowColor = new Color(1f, 0.92f, 0f, 0.5f);
    private bool isActive = false;
    private Fire_Bolt fireBolt;
    private GameObject glowEffect;
    private SpriteRenderer glowSprite;

    private void Start()
    {
        FindWeaponComponent();
        CreateGlowEffect();
    }

    private void CreateGlowEffect()
    {
        if (GameManager.Instance?.Player == null) return;

        glowEffect = new GameObject("GlowEffect");
        glowEffect.transform.SetParent(GameManager.Instance.Player.transform);
        glowEffect.transform.localPosition = Vector3.zero;
        glowEffect.transform.localScale = Vector3.one * 1.2f; // Slightly larger than player

        glowSprite = glowEffect.AddComponent<SpriteRenderer>();
        glowSprite.sprite = GameManager.Instance.Player.GetComponent<SpriteRenderer>()?.sprite;
        glowSprite.color = glowColor;
        glowSprite.sortingOrder = -1; // Behind player
        glowSprite.enabled = false;
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

        if (glowSprite != null)
        {
            glowSprite.enabled = true;
            StartCoroutine(PulseGlow());
        }

        Debug.Log($"FireRateBooster active for {duration} seconds");
        yield return new WaitForSeconds(duration);

        Debug.Log($"Restoring fire rate back to {originalFireRate}");
        fireBolt.baserate = originalFireRate;
        if (glowSprite != null)
        {
            glowSprite.enabled = false;
        }
        isActive = false;

        Debug.Log("FireRateBooster effect complete, clearing from inventory");
        PlayerSpellManager spellManager = GameManager.Instance.Player.GetComponent<PlayerSpellManager>();
        if (spellManager != null)
        {
            spellManager.ClearUtilitySpell();
        }
    }

    private IEnumerator PulseGlow()
    {
        float pulseSpeed = 2f;
        float minAlpha = 0.3f;
        float maxAlpha = 0.7f;

        while (isActive && glowSprite != null)
        {
            float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
            Color newColor = glowColor;
            newColor.a = Mathf.Lerp(minAlpha, maxAlpha, t);
            glowSprite.color = newColor;
            yield return null;
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
        }
        if (glowEffect != null)
        {
            Destroy(glowEffect);
        }
    }
}