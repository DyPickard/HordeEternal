using UnityEngine;
using System.Collections;

public class PlayerSpellManager : MonoBehaviour
{
    public Transform spellSlot; // Empty GameObject where spell script will go
    private UIManager uiManager;
    public Sprite fireBoltIcon; // Icon to display
    public GameObject fireBoltProjectilePrefab; // Drag your projectile prefab here


    private Spell currentSpell;

    void Start()
    {
        StartCoroutine(DelayedEquip());
    }

    IEnumerator DelayedEquip()
    {
        yield return null;

        uiManager = FindObjectOfType<UIManager>();

        if (uiManager != null)
        {
            Debug.Log("UIManager found, equipping spell.");
            EquipSpell(fireBoltIcon, typeof(Fire_Bolt));
        }
        else
        {
            Debug.LogError("UIManager not found!");
        }
    }

    public void EquipSpell(Sprite icon, System.Type spellType)
    {
        // Remove old spell
        if (currentSpell != null)
        {
            Destroy(currentSpell.gameObject);
        }

        // Add new spell
        GameObject newSpellGO = new GameObject("EquippedSpell");
        newSpellGO.transform.parent = spellSlot;
        newSpellGO.transform.localPosition = Vector3.zero;

        currentSpell = (Spell)newSpellGO.AddComponent(spellType);

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
}