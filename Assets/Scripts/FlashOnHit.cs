using UnityEngine;
using System.Collections;
using System;

public class FlashOnHit : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashCoroutine;

    public event Action OnFlashComplete;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void Flash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        if (spriteRenderer == null) yield break;

        Color flashColor = Color.red;
        flashColor.a = spriteRenderer.color.a;
        float flashDuration = 0.1f;
        int flashCount = 3;

        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }

        spriteRenderer.color = originalColor;
        flashCoroutine = null;

        OnFlashComplete?.Invoke();
    }

    public void UpdateOriginalColor(Color newColor)
    {
        originalColor = newColor;
    }
}