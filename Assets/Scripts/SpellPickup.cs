using UnityEngine;

public class SpellPickup : MonoBehaviour
{
    public Sprite icon;
    public bool isUtility;
    public SpellType spellType;
    public string itemName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerSpellManager manager = other.GetComponent<PlayerSpellManager>();
        if (manager == null) return;

        if (isUtility && manager.HasUtilitySpell())
        {
            Debug.Log($"Cannot pick up {itemName} - already have a utility spell equipped!");
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
                InventoryManager.Instance.EquipItem(itemName, InventorySlotType.Utility);
            }
        }
        else
        {
            manager.EquipWeaponSpell(icon, typeToEquip);
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
                return typeof(SpellShield);
            case SpellType.FireRateBooster:
                return typeof(FireRateBooster);
            case SpellType.Shockwave:
                return typeof(Shockwave);
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
    FireRateBooster,
    Shockwave
}