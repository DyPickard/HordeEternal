using UnityEngine;
using System.Collections;

public class AfterImageSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float fadeStartDelay = 0.0f;
    private float fadeDuration = 0.5f;
    private Color startColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("AfterImageSprite: No SpriteRenderer found!");
            Destroy(gameObject);
            return;
        }

        startColor = spriteRenderer.color;
        Debug.Log($"AfterImageSprite started with color: {startColor}");
        StartCoroutine(FadeAndDestroy());
    }

    private IEnumerator FadeAndDestroy()
    {
        if (fadeStartDelay > 0)
        {
            yield return new WaitForSeconds(fadeStartDelay);
        }

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0f, elapsedTime / fadeDuration);
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            spriteRenderer.color = newColor;
            Debug.Log($"AfterImageSprite alpha: {alpha}");
            yield return null;
        }

        Debug.Log("AfterImageSprite faded out, destroying");
        Destroy(gameObject);
    }
}