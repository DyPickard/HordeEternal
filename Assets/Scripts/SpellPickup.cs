using UnityEngine;

public class SpellPickup : MonoBehaviour
{
    public Sprite icon;
    public bool isUtility;
    public SpellType spellType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerSpellManager manager = other.GetComponent<PlayerSpellManager>();
        if (manager == null) return;

        System.Type typeToEquip = GetSpellType();

        if (isUtility)
            manager.EquipUtilitySpell(icon, typeToEquip);
        else
            manager.EquipWeaponSpell(icon, typeToEquip);

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
            // Add more spell types as needed
            default:
                Debug.LogWarning("Unknown spell type, defaulting to Fire_Bolt.");
                return typeof(Fire_Bolt);
        }
    }
}

public enum SpellType
{
    FireBolt,
    SpellShield
    // Add more entries here as you add more spells
}