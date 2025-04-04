using UnityEngine;

public class SpellPickup : MonoBehaviour
{
    public Sprite icon;
    public bool isUtility;
    public SpellType spellType;
    public string itemName;
    public GameObject projectilePrefab;
    public AudioClip pickupSound;

    void Start()
    {
        pickupSound = Resources.Load<AudioClip>("Spells/Pickup");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerSpellManager manager = other.GetComponent<PlayerSpellManager>();
        if (manager == null) return;

        if (isUtility && manager.HasUtilitySpell())
        {
            Debug.Log("Cannot pick up shield - already have a utility spell equipped!");
            return;
        }

        InventorySlotType slotType = isUtility ? InventorySlotType.Utility : InventorySlotType.Weapon;

        if (InventoryManager.Instance != null && !InventoryManager.Instance.CanPickupItem(itemName, slotType))
        {
            Debug.Log($"Cannot pick up {itemName} - already have one equipped!");
            return;
        }

        System.Type typeToEquip = GetSpellType();

        if (isUtility)
        {
            manager.EquipUtilitySpell(icon, typeToEquip);
            if (InventoryManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(pickupSound);
                InventoryManager.Instance.EquipItem(itemName, InventorySlotType.Utility);
            }
        }
        else
        {
            manager.ClearWeaponSpell();
            manager.EquipWeaponSpell(icon, typeToEquip, projectilePrefab);
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.EquipItem(itemName, InventorySlotType.Weapon);
            }
        }

        Destroy(gameObject);
    }

    private System.Type GetSpellType()
    {
        switch (spellType)
        {
            case SpellType.FireBolt:
                return typeof(Fire_Bolt);
            case SpellType.SpellShield:
                return typeof(ShieldSpell);
            case SpellType.LightningBolt:
                return typeof(Lightning_Bolt);
            case SpellType.IceBolt:
                return typeof(Ice_Bolt);
            default:
                Debug.LogWarning("Unknown spell type, defaulting to Fire_Bolt.");
                return typeof(Fire_Bolt);
        }
    }
}

public enum SpellType
{
    FireBolt,
    SpellShield,
    LightningBolt,
    IceBolt
    // Add more entries here as you add more spells
}