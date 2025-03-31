using UnityEngine;

[System.Serializable]
public class DroppableItem
{
    public GameObject itemPrefab;
    public string itemName;
    [Range(0f, 100f)]
    public float dropChance;
    public bool isEnabled = true;

    public DroppableItem(GameObject prefab, string name, float chance)
    {
        itemPrefab = prefab;
        itemName = name;
        dropChance = Mathf.Clamp(chance, 0f, 100f);
    }
}