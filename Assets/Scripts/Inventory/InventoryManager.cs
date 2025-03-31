using UnityEngine;
using System.Collections.Generic;

public enum InventorySlotType
{
    Weapon,
    Utility,
    Mobility
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private Dictionary<InventorySlotType, string> equippedItems = new Dictionary<InventorySlotType, string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (InventorySlotType slotType in System.Enum.GetValues(typeof(InventorySlotType)))
        {
            equippedItems[slotType] = "";
        }
    }

    public bool CanPickupItem(string itemName, InventorySlotType slotType)
    {
        if (string.IsNullOrEmpty(equippedItems[slotType]))
            return true;

        return equippedItems[slotType] != itemName;
    }

    public void EquipItem(string itemName, InventorySlotType slotType)
    {
        equippedItems[slotType] = itemName;
        Debug.Log($"Equipped {itemName} to {slotType} slot");
    }

    public void ClearSlot(InventorySlotType slotType)
    {
        equippedItems[slotType] = "";
        Debug.Log($"Cleared {slotType} slot");
    }

    public string GetEquippedItem(InventorySlotType slotType)
    {
        return equippedItems[slotType];
    }
}