using UnityEngine;

public enum WeaponSpellType
{
    FireBolt,
    IceBolt,
    LightningBolt
}

public class WeaponPickup : MonoBehaviour
{
    public Sprite icon;
    public WeaponSpellType spellType;
    public string itemName;
    public GameObject projectilePrefab;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerSpellManager spellManager = other.GetComponent<PlayerSpellManager>();
        if (spellManager == null) return;

        // Convert enum to the actual spell type
        System.Type weaponType = GetWeaponType();

        // Equip the new weapon spell
        spellManager.EquipWeaponSpell(icon, weaponType, projectilePrefab);

        Destroy(gameObject);
    }

    private System.Type GetWeaponType()
    {
        switch (spellType)
        {
            case WeaponSpellType.FireBolt:
                return typeof(Fire_Bolt);
            //case WeaponSpellType.IceBolt:
            //    return typeof(Ice_Bolt);
            //case WeaponSpellType.LightningBolt:
            //    return typeof(Lightning_Bolt);
            default:
                Debug.LogWarning("Only FireBolt is currently implemented, defaulting to Fire_Bolt");
                return typeof(Fire_Bolt);
        }
    }
}