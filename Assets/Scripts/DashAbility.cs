using UnityEngine;
using System.Collections;

public class QuickDashAbility : MovementAbility
{
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashDuration = 0.2f;
    private bool isDashing = false;

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

        float startTime = Time.time;
        Vector2 startPosition = player.transform.position;
        Vector2 targetPosition = startPosition + direction * dashDistance;

        // Temporarily disable player control during dash
        bool wasControlEnabled = playerMovement.enabled;
        playerMovement.enabled = false;

        // Make player temporarily invulnerable
        GameManager.Instance.SetInvulnerable(true);

        while (Time.time < startTime + dashDuration)
        {
            float t = (Time.time - startTime) / dashDuration;
            player.transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        player.transform.position = targetPosition;

        playerMovement.enabled = wasControlEnabled;

        GameManager.Instance.SetInvulnerable(false);

        isDashing = false;
        Debug.Log("Quick dash completed!");
    }
}