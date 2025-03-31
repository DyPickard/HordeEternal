using UnityEngine;

public class MovementPickup : MonoBehaviour
{
    public Sprite icon;
    public MovementAbilityType abilityType;
    public string itemName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        MovementAbility currentAbility = other.GetComponent<MovementAbility>();

        if (currentAbility != null && !currentAbility.CanUseAbility())
        {
            Debug.Log("Cannot pick up movement ability during cooldown!");
            return;
        }

        GameManager gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.SwitchMovementAbility(abilityType);

            MovementAbility newAbility = other.GetComponent<MovementAbility>();
            if (newAbility != null)
            {
                newAbility.StartCooldownOnPickup();
            }

            Destroy(gameObject);
        }
    }
}