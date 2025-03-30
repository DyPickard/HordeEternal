using UnityEngine;
using System.Collections;

public class ChargeAbility : MovementAbility
{
    [SerializeField] private float speedMultiplier = 1.25f; // 25% speed increase
    [SerializeField] private float chargeDuration = 3f;
    private bool isCharging = false;

    protected override void Start()
    {
        base.Start();
        cooldown = 10f; // Longer cooldown as specified
    }

    public override void UseAbility()
    {
        if (!CanUseAbility() || isCharging) return;

        StartCoroutine(PerformCharge());
        StartCooldown();
    }

    private IEnumerator PerformCharge()
    {
        Debug.Log("Starting charge boost!");
        isCharging = true;

        float originalSpeed = playerMovement.speed;
        playerMovement.speed *= speedMultiplier; // Multiply by 1.25 for 25% increase


        // Restore original speed
        playerMovement.speed = originalSpeed;

        isCharging = false;
        Debug.Log($"Charge boost ended! Speed returned to {originalSpeed}");
    }
}