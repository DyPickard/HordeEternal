using UnityEngine;
using System.Collections;

public class TeleportAbility : MovementAbility
{
    [SerializeField] private float maxTeleportDistance = 5f;
    [SerializeField] private float fadeDuration = 0.2f;
    private bool isAiming = false;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        mainCamera = Camera.main;
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on player!");
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.LeftShift) && CanUseAbility())
        {
            StartAiming();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && isAiming)
        {
            FinishAiming();
        }
    }

    private void StartAiming()
    {
        isAiming = true;
        Debug.Log("Started aiming teleport...");
    }

    private void FinishAiming()
    {
        if (!CanUseAbility()) return;

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 currentPosition = player.transform.position;
        Vector2 teleportDirection = mousePosition - currentPosition;

        // Limit teleport distance
        if (teleportDirection.magnitude > maxTeleportDistance)
        {
            teleportDirection = teleportDirection.normalized * maxTeleportDistance;
        }

        StartCoroutine(PerformTeleportWithFade(currentPosition + teleportDirection));
        StartCooldown();
        isAiming = false;
    }

    public override void UseAbility()
    {
        return;
    }

    private IEnumerator PerformTeleportWithFade(Vector2 targetPosition)
    {
        Debug.Log($"Teleporting to: {targetPosition}");

        // Temporarily disable player control and make invulnerable
        bool wasControlEnabled = playerMovement.enabled;
        playerMovement.enabled = false;
        GameManager.Instance.SetInvulnerable(true);

        // Fade out
        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        // Perform teleport
        player.transform.position = targetPosition;

        // Fade in
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            spriteRenderer.color = Color.Lerp(endColor, startColor, t);
            yield return null;
        }

        // Ensure color is fully reset
        spriteRenderer.color = startColor;

        // Restore control and vulnerability
        playerMovement.enabled = wasControlEnabled;
        GameManager.Instance.SetInvulnerable(false);

        Debug.Log("Teleport completed!");
    }
}