using UnityEngine;
using System.Collections;

public class SpellShield : UtilitySpell
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private Color shieldColor = new Color(0.5f, 0.8f, 1f, 0.5f); // Light blue, semi-transparent
    [SerializeField] private float shieldScale = 1.5f; // How much bigger than the player
    private bool isActive = false;
    private GameObject shieldVisual;
    private SpriteRenderer shieldRenderer;

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("SpellShield: GameManager.Instance is null!");
            return;
        }

        // Create shield visual
        CreateShieldVisual();
    }

    private void CreateShieldVisual()
    {
        if (GameManager.Instance == null || GameManager.Instance.Player == null)
        {
            Debug.LogError("Cannot create shield visual - GameManager or Player is null!");
            return;
        }

        shieldVisual = new GameObject("ShieldVisual");
        shieldVisual.transform.SetParent(GameManager.Instance.Player.transform);
        shieldVisual.transform.localPosition = Vector3.zero;

        shieldRenderer = shieldVisual.AddComponent<SpriteRenderer>();
        shieldRenderer.sprite = GameManager.Instance.spellShieldPrefab.GetComponent<SpriteRenderer>()?.sprite;
        shieldRenderer.color = shieldColor;
        shieldRenderer.sortingOrder = 3; // Make sure it renders above the player

        shieldVisual.transform.localScale = Vector3.one * shieldScale;

        shieldRenderer.enabled = false;
    }

    public override void Activate()
    {
        Debug.Log("SpellShield Activate() called");
        if (!isActive)
        {
            if (GameManager.Instance != null)
            {
                if (shieldVisual == null)
                {
                    CreateShieldVisual();
                }

                if (shieldVisual != null)
                {
                    Debug.Log("Starting shield effect");
                    StartCoroutine(ActivateShield());
                }
                else
                {
                    Debug.LogError("Cannot activate shield - shield visual could not be created!");
                }
            }
            else
            {
                Debug.LogError("Cannot activate shield - GameManager.Instance is null!");
            }
        }
        else
        {
            Debug.Log("Shield is already active!");
        }
    }

    private IEnumerator ActivateShield()
    {
        isActive = true;

        // Enable shield effect and visual
        GameManager.Instance.isShielded = true;
        if (shieldRenderer != null)
        {
            shieldRenderer.enabled = true;
            StartCoroutine(PulseShieldEffect());
        }
        Debug.Log("Shield activated for " + duration + " seconds");

        yield return new WaitForSeconds(duration);

        GameManager.Instance.isShielded = false;
        if (shieldRenderer != null)
        {
            shieldRenderer.enabled = false;
        }
        Debug.Log("Shield deactivated");
        isActive = false;

        Debug.Log("Shield effect complete, clearing from inventory");
        PlayerSpellManager spellManager = GetComponentInParent<PlayerSpellManager>();
        if (spellManager != null)
        {
            spellManager.ClearUtilitySpell();
        }
    }

    private IEnumerator PulseShieldEffect()
    {
        float pulseSpeed = 2f;
        float minScale = shieldScale * 0.9f;
        float maxScale = shieldScale * 1.1f;

        while (isActive)
        {
            float pulseFactor = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            float currentScale = Mathf.Lerp(minScale, maxScale, pulseFactor);
            shieldVisual.transform.localScale = Vector3.one * currentScale;

            Color currentColor = shieldColor;
            currentColor.a = Mathf.Lerp(0.3f, 0.7f, pulseFactor);
            shieldRenderer.color = currentColor;

            yield return null;
        }
    }

    private void OnDestroy()
    {
        if (shieldVisual != null)
        {
            Destroy(shieldVisual);
        }
    }
}