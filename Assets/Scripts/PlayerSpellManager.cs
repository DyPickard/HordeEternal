using UnityEngine;
using System.Collections;

public class PlayerSpellManager : MonoBehaviour
{
    public Transform spellSlot;
    private UIManager uiManager;

    public Sprite weaponSpellIcon; // Icon to display
    public Sprite utilitySpellIcon;

    [Header("Default Weapon Spell")]
    public Sprite defaultWeaponIcon;
    public GameObject defaultProjectilePrefab;
    public SpellType defaultSpellType = SpellType.FireBolt;

    [Header("Projectile Prefabs")]
    public GameObject fireBoltProjectilePrefab;
    public GameObject lightningBoltProjectilePrefab;
    public GameObject iceBoltProjectilePrefab;
    public GameObject utilitySpellPrefab;

    [Header("Weapon Spell Sounds")]
    public AudioClip fireballSound;
    public AudioClip iceSound;
    public AudioClip lightningSound;

    private WeaponSpell currentSpell;
    private UtilitySpell utilitySpell;

    void Start()
    {
        foreach (WeaponSpell spell in GetComponentsInChildren<WeaponSpell>())
        {
            Destroy(spell.gameObject);
        }
        StartCoroutine(DelayedSetup());
    }

    IEnumerator DelayedSetup()
    {
        yield return new WaitForSeconds(0.1f);

        uiManager = FindObjectOfType<UIManager>();

        if (uiManager != null && currentSpell == null)
        {
            Debug.Log("UIManager found. Equipping default spell.");
            EquipDefaultWeaponSpell();
        }
        else
        {
            Debug.LogError("UIManager not found!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && utilitySpell != null)
        {
            utilitySpell.Activate();
        }
    }

    public void EquipWeaponSpell(Sprite icon, System.Type spellType, GameObject projectilePrefab)
    {
        if (currentSpell != null)
        {
            Destroy(currentSpell.gameObject);
        }

        GameObject newSpellGO = new GameObject("EquippedWeaponSpell");
        newSpellGO.transform.SetParent(spellSlot, false);
        currentSpell = (WeaponSpell)newSpellGO.AddComponent(spellType);

        if (currentSpell is Fire_Bolt fireBolt)
        {
            fireBolt.playerLevel = GetComponent<PlayerLevel>();
            fireBolt.firePosition = transform;
            fireBolt.proj = projectilePrefab;
            fireBolt.SetSound(fireballSound);
        }
        else if (currentSpell is Ice_Bolt iceBolt)
        {
            iceBolt.playerLevel = GetComponent<PlayerLevel>();
            iceBolt.firePosition = transform;
            iceBolt.ice_proj = projectilePrefab;
            iceBolt.SetSound(iceSound);
        }
        else if (currentSpell is Lightning_Bolt lightningBolt)
        {
            lightningBolt.playerLevel = GetComponent<PlayerLevel>();
            lightningBolt.firePosition = transform;
            lightningBolt.lightning_proj = projectilePrefab;
            lightningBolt.SetSound(lightningSound);
        }

        uiManager.SetWeaponPowerUp(icon);
    }

    public void EquipUtilitySpell(Sprite icon, System.Type spellType)
    {
        if (utilitySpell != null)
        {
            Destroy(utilitySpell.gameObject);
        }

        GameObject newSpellGO = new GameObject("EquippedUtilitySpell");
        newSpellGO.transform.SetParent(spellSlot, false);
        utilitySpell = (UtilitySpell)newSpellGO.AddComponent(spellType);

        uiManager.SetUtilityPowerUp(icon);
    }

    public void ClearUtilitySpell()
    {
        utilitySpell = null;
        uiManager.SetUtilityPowerUp(null);

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ClearSlot(InventorySlotType.Utility);
        }
    }

    public void ClearWeaponSpell()
    {
        if (currentSpell != null)
        {
            Destroy(currentSpell.gameObject);
            currentSpell = null;
        }

        uiManager.SetWeaponPowerUp(null);

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ClearSlot(InventorySlotType.Weapon);
        }
    }

    public bool HasUtilitySpell()
    {
        return utilitySpell != null;
    }

    private void EquipDefaultWeaponSpell()
    {
        System.Type spellType = GetSpellClass(defaultSpellType);
        GameObject proj = GetProjectileForType(defaultSpellType);

        EquipWeaponSpell(defaultWeaponIcon, spellType, proj);
    }

    private System.Type GetSpellClass(SpellType type)
    {
        switch (type)
        {
            case SpellType.FireBolt: return typeof(Fire_Bolt);
            case SpellType.IceBolt: return typeof(Ice_Bolt);
            case SpellType.LightningBolt: return typeof(Lightning_Bolt);
            default:
                Debug.LogWarning("Unknown spell type. Defaulting to Fire_Bolt.");
                return typeof(Fire_Bolt);
        }
    }

    private GameObject GetProjectileForType(SpellType type)
    {
        switch (type)
        {
            case SpellType.FireBolt: return fireBoltProjectilePrefab;
            case SpellType.IceBolt: return iceBoltProjectilePrefab;
            case SpellType.LightningBolt: return lightningBoltProjectilePrefab;
            default:
                Debug.LogWarning("Unknown projectile type. Defaulting to FireBolt prefab.");
                return fireBoltProjectilePrefab;
        }
    }
}