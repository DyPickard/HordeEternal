using UnityEngine;

public class ItemLifetime : MonoBehaviour
{
    public float totalLifetime = 10f;
    public float fadeStartTime = 3f;

    private float timeAlive = 0f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private const float pulseMin = 0.7f;
    private const float pulseMax = 1.0f;
    private const int numberOfPulses = 3;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogError("No SpriteRenderer found for item fade effect!");
        }
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;

        if (timeAlive >= (totalLifetime - fadeStartTime))
        {
            float timeInFadePhase = timeAlive - (totalLifetime - fadeStartTime);
            float pulseProgress = (timeInFadePhase / fadeStartTime) * numberOfPulses;

            float pulseFactor = Mathf.PingPong(pulseProgress * 2f, 1f);
            float alpha = Mathf.Lerp(pulseMin, pulseMax, pulseFactor);

            if (spriteRenderer != null)
            {
                Color newColor = originalColor;
                newColor.a = alpha;
                spriteRenderer.color = newColor;
            }
        }

        if (timeAlive >= totalLifetime)
        {
            Destroy(gameObject);
        }
    }
}