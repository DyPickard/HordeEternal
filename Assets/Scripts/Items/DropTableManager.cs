using UnityEngine;
using System.Collections.Generic;

public class DropTableManager : MonoBehaviour
{
    public static DropTableManager Instance { get; private set; }

    [SerializeField]
    private List<DroppableItem> dropTable = new List<DroppableItem>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItemToDropTable(GameObject itemPrefab, string itemName, float dropChance)
    {
        var existingItem = dropTable.Find(item => item.itemName == itemName);
        if (existingItem != null)
        {
            Debug.Log($"Updating existing item {itemName} in drop table");
            existingItem.itemPrefab = itemPrefab;
            existingItem.dropChance = dropChance;
            return;
        }

        DroppableItem newItem = new DroppableItem(itemPrefab, itemName, dropChance);
        dropTable.Add(newItem);
        Debug.Log($"Added new item {itemName} to drop table with {dropChance}% chance");
    }

    public void RemoveItemFromDropTable(string itemName)
    {
        dropTable.RemoveAll(item => item.itemName == itemName);
    }

    public void HandleEnemyDeath(Vector3 deathPosition)
    {
        Debug.Log($"Handling enemy death at {deathPosition}. Items in drop table: {dropTable.Count}");

        List<DroppableItem> possibleDrops = new List<DroppableItem>();

        foreach (DroppableItem item in dropTable)
        {
            if (!item.isEnabled)
            {
                Debug.Log($"Item {item.itemName} is disabled, skipping");
                continue;
            }

            float randomRoll = Random.Range(0f, 100f);
            Debug.Log($"Rolling for {item.itemName}: got {randomRoll}, need <= {item.dropChance}");

            if (randomRoll <= item.dropChance)
            {
                possibleDrops.Add(item);
            }
        }

        if (possibleDrops.Count > 0)
        {
            DroppableItem selectedDrop = possibleDrops[Random.Range(0, possibleDrops.Count)];
            Vector3 randomOffset = Random.insideUnitCircle * 0.5f;
            Vector3 spawnPosition = deathPosition + new Vector3(randomOffset.x, randomOffset.y, 0);

            Debug.Log($"Spawning {selectedDrop.itemName} at {spawnPosition}");
            GameObject spawnedItem = Instantiate(selectedDrop.itemPrefab, spawnPosition, Quaternion.identity);

            ItemLifetime lifetime = spawnedItem.AddComponent<ItemLifetime>();
            lifetime.totalLifetime = 10f;
            lifetime.fadeStartTime = 3f;
        }
        else
        {
            Debug.Log("No items were selected to drop");
        }
    }

    public void SetItemEnabled(string itemName, bool enabled)
    {
        var item = dropTable.Find(i => i.itemName == itemName);
        if (item != null)
        {
            item.isEnabled = enabled;
        }
    }

    public void ClearDropTable()
    {
        dropTable.Clear();
    }

    public int GetDropTableCount()
    {
        return dropTable.Count;
    }
}