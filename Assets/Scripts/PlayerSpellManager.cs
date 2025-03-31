using UnityEngine;
using System.Collections;

public class PlayerSpellManager : MonoBehaviour
{
    public Transform spellSlot; // Empty GameObject where spell script will go
    private UIManager uiManager;
    public Sprite fireBoltIcon; // Icon to display
    public Sprite utilitySpellIcon;
    public GameObject fireBoltProjectilePrefab; // Drag your projectile prefab here
    public GameObject utilitySpellPrefab;

    private WeaponSpell currentSpell;
    private UtilitySpell utilitySpell;

    void Start()
    {
        uiManager = UnityEngine.Object.FindAnyObjectByType<UIManager>();
        StartCoroutine(DelayedEquip());
    }

    IEnumerator DelayedEquip()
    {
        yield return null;

        if (uiManager != null)
        {
            Debug.Log("UIManager found, equipping spell.");
            EquipWeaponSpell(fireBoltIcon, typeof(Fire_Bolt));
        }
        else
        {
            Debug.LogError("UIManager not found!");
        }
    }

    void Update()
    {
        // Trigger utility spell with Spacebar
        if (Input.GetKeyDown(KeyCode.Space) && utilitySpell != null)
        {
            utilitySpell.Activate();
        }
    }

    public void EquipWeaponSpell(Sprite icon, System.Type spellType)
    {
        // Remove old spell
        if (currentSpell != null)
        {
            Destroy(currentSpell.gameObject);
        }

        // Add new spell
        GameObject newSpellGO = new GameObject("EquippedWeaponSpell");
        newSpellGO.transform.parent = spellSlot;
        newSpellGO.transform.localPosition = Vector3.zero;

        currentSpell = (WeaponSpell)newSpellGO.AddComponent(spellType);

        // Optional: if the spell needs setup like `playerLevel`, assign it here
        if (currentSpell is Fire_Bolt fireBolt)
        {
            fireBolt.playerLevel = GetComponent<PlayerLevel>(); // Assuming on Player
            fireBolt.firePosition = transform; // Set correct fire position
            fireBolt.proj = fireBoltProjectilePrefab;
        }

        // Update UI
        uiManager.SetWeaponPowerUp(icon);
    }

    public void EquipUtilitySpell(Sprite icon, System.Type spellType)
    {
        if (utilitySpell != null)
            Destroy(utilitySpell.gameObject);

        GameObject newSpellGO = new GameObject("EquippedUtilitySpell");
        newSpellGO.transform.parent = spellSlot;
        newSpellGO.transform.localPosition = Vector3.zero;

        utilitySpell = (UtilitySpell)newSpellGO.AddComponent(spellType);

        uiManager.SetUtilityPowerUp(icon);
    }

    public bool HasUtilitySpell()
    {
        return utilitySpell != null;
    }

    public void ClearUtilitySpell()
    {
        utilitySpell = null;
        uiManager.SetUtilityPowerUp(null); // this will make it transparent if sprite is null
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
}