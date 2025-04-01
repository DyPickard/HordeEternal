using UnityEngine;

public class SpellSetup : MonoBehaviour
{
    [Header("Utility Spell Prefabs")]
    public GameObject fireRateBoosterPickupPrefab;
    public GameObject shockwavePickupPrefab;

    [Header("Drop Chances")]
    [Range(0, 100)]
    public float fireRateBoosterDropChance = 15f;
    [Range(0, 100)]
    public float shockwaveDropChance = 15f;

    void Start()
    {
        if (DropTableManager.Instance != null)
        {
            if (fireRateBoosterPickupPrefab != null)
            {
                DropTableManager.Instance.AddItemToDropTable(
                    fireRateBoosterPickupPrefab,
                    "Fire Rate Booster",
                    fireRateBoosterDropChance
                );
            }

            if (shockwavePickupPrefab != null)
            {
                DropTableManager.Instance.AddItemToDropTable(
                    shockwavePickupPrefab,
                    "Shockwave",
                    shockwaveDropChance
                );
            }
        }
    }
}