using UnityEngine;
using System.Collections;

public abstract class MovementAbility : MonoBehaviour
{
    [SerializeField] protected float cooldown = 3f;
    protected bool isReady = true;
    protected float cooldownTimer = 0f;
    protected PlayerMovement playerMovement;
    protected GameObject player;

    protected virtual void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        player = gameObject;
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found on object with MovementAbility!");
        }
    }

    protected virtual void Update()
    {
        // Handle cooldown
        if (!isReady)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                isReady = true;
                OnAbilityReady();
            }
        }
    }

    public bool CanUseAbility()
    {
        return isReady;
    }

    protected void StartCooldown()
    {
        isReady = false;
        cooldownTimer = cooldown;
    }

    public abstract void UseAbility();
    protected virtual void OnAbilityReady()
    {
        Debug.Log($"{GetType().Name} is ready!");
    }
}