using UnityEngine;
using System.Collections;

public class TeleportAbility : MovementAbility
{
    [SerializeField] private float maxTeleportDistance = 5f;
    private bool isAiming = false;
    private Camera mainCamera;

    protected override void Start()
    {
        base.Start();
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space) && CanUseAbility())
        {
            StartAiming();
        }
        else if (Input.GetKeyUp(KeyCode.Space) && isAiming)
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

        PerformTeleport(currentPosition + teleportDirection);
        StartCooldown();
        isAiming = false;
    }

    public override void UseAbility()
    {
        return;
    }

    private void PerformTeleport(Vector2 targetPosition)
    {
        Debug.Log($"Teleporting to: {targetPosition}");

        // Temporarily disable player control and make invulnerable
        bool wasControlEnabled = playerMovement.enabled;
        playerMovement.enabled = false;
        GameManager.Instance.SetInvulnerable(true);

        // Instant teleport
        player.transform.position = targetPosition;

        // Restore control and vulnerability
        playerMovement.enabled = wasControlEnabled;
        GameManager.Instance.SetInvulnerable(false);

        Debug.Log("Teleport completed!");
    }
}