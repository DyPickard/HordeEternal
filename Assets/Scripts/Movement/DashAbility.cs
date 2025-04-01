using UnityEngine;
using System.Collections;

public class QuickDashAbility : MovementAbility
{
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float afterImageSpawnInterval = 0.02f;
    [SerializeField] private AudioClip dashSound;
    private bool isDashing = false;
    private SpriteRenderer playerSprite;

    protected override void Start()
    {
        base.Start();
        dashSound = Resources.Load<AudioClip>("Spells/Whoosh");
        playerSprite = GetComponent<SpriteRenderer>();
        if (playerSprite == null)
        {
            Debug.LogError("SpriteRenderer not found! After-images won't work.");
        }
        else
        {
            Debug.Log("Found SpriteRenderer for after-images!");
        }
    }

    public override void UseAbility()
    {
        if (!CanUseAbility() || isDashing) return;

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
            StartCooldown();
        }
        else
        {
            Debug.Log("No valid dash direction found!");
        }
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        Debug.Log("Quick dashing in direction: " + direction);
        isDashing = true;
        AudioManager.Instance.PlaySFX(dashSound);

        float startTime = Time.time;
        Vector2 startPosition = player.transform.position;
        Vector2 targetPosition = startPosition + direction * dashDistance;

        // Temporarily disable player control during dash
        bool wasControlEnabled = playerMovement.enabled;
        playerMovement.enabled = false;

        // Make player temporarily invulnerable
        GameManager.Instance.SetInvulnerable(true);

        float nextAfterImageTime = startTime;

        while (Time.time < startTime + dashDuration)
        {
            float t = (Time.time - startTime) / dashDuration;
            player.transform.position = Vector2.Lerp(startPosition, targetPosition, t);

            // Spawn after-image
            if (Time.time >= nextAfterImageTime && playerSprite != null)
            {
                SpawnAfterImage();
                nextAfterImageTime = Time.time + afterImageSpawnInterval;
            }

            yield return null;
        }

        player.transform.position = targetPosition;
        playerMovement.enabled = wasControlEnabled;
        GameManager.Instance.SetInvulnerable(false);
        isDashing = false;
        Debug.Log("Quick dash completed!");
    }

    private void SpawnAfterImage()
    {
        GameObject afterImage = new GameObject("AfterImage");
        afterImage.transform.position = player.transform.position;
        afterImage.transform.rotation = player.transform.rotation;
        afterImage.transform.localScale = player.transform.localScale;

        SpriteRenderer afterImageSprite = afterImage.AddComponent<SpriteRenderer>();
        afterImageSprite.sprite = playerSprite.sprite;
        afterImageSprite.sortingLayerName = playerSprite.sortingLayerName;
        afterImageSprite.sortingOrder = playerSprite.sortingOrder + 1;
        afterImageSprite.color = new Color(0.7f, 0.7f, 1f, 0.7f);
        afterImageSprite.flipX = playerSprite.flipX;
        afterImageSprite.maskInteraction = playerSprite.maskInteraction;
        afterImageSprite.material = playerSprite.material;

        Debug.Log($"After-image created with: Sprite={afterImageSprite.sprite != null}, " +
                  $"Layer={afterImageSprite.sortingLayerName}, " +
                  $"Order={afterImageSprite.sortingOrder}, " +
                  $"Position={afterImage.transform.position}");

        afterImage.AddComponent<AfterImageSprite>();
    }
}