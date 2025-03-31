using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject skeletonPrefab;
    [SerializeField] private GameObject ogrePrefab;
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnDistanceFromCamera = 2f;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;

    [Header("Spawn Weights")]
    [SerializeField][Range(0, 100)] private float skeletonSpawnWeight = 70f;
    [SerializeField][Range(0, 100)] private float ogreSpawnWeight = 20f;
    [SerializeField][Range(0, 100)] private float ghostSpawnWeight = 10f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = GetRandomSpawnPosition();

        // Randomly choose enemy type based on weights
        float totalWeight = skeletonSpawnWeight + ogreSpawnWeight + ghostSpawnWeight;
        float randomValue = Random.Range(0f, totalWeight);

        GameObject enemyToSpawn;
        if (randomValue <= skeletonSpawnWeight)
        {
            enemyToSpawn = skeletonPrefab;
        }
        else if (randomValue <= skeletonSpawnWeight + ogreSpawnWeight)
        {
            enemyToSpawn = ogrePrefab;
        }
        else
        {
            enemyToSpawn = ghostPrefab;
            spawnPos = GetRandomGhostSpawnPosition();
        }

        Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);
    }

    Vector3 GetRandomGhostSpawnPosition()
    {
        Vector3 cameraPos = mainCamera.transform.position;
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        float x = 0f, y = 0f;
        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: x = Random.Range(-camWidth / 2, camWidth / 2); y = camHeight / 2 + spawnDistanceFromCamera; break;
            case 1: x = Random.Range(-camWidth / 2, camWidth / 2); y = -camHeight / 2 - spawnDistanceFromCamera; break;
            case 2: x = -camWidth / 2 - spawnDistanceFromCamera; y = Random.Range(-camHeight / 2, camHeight / 2); break;
            case 3: x = camWidth / 2 + spawnDistanceFromCamera; y = Random.Range(-camHeight / 2, camHeight / 2); break;
        }

        Vector3 worldPos = new Vector3(x, y, 0) + cameraPos;
        worldPos.z = 0f;
        return worldPos;
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 cameraPos = mainCamera.transform.position;
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        for (int i = 0; i < 10; i++) // Try up to 10 times
        {
            float x = 0f, y = 0f;
            int side = Random.Range(0, 4);

            switch (side)
            {
                case 0: x = Random.Range(-camWidth / 2, camWidth / 2); y = camHeight / 2 + spawnDistanceFromCamera; break;
                case 1: x = Random.Range(-camWidth / 2, camWidth / 2); y = -camHeight / 2 - spawnDistanceFromCamera; break;
                case 2: x = -camWidth / 2 - spawnDistanceFromCamera; y = Random.Range(-camHeight / 2, camHeight / 2); break;
                case 3: x = camWidth / 2 + spawnDistanceFromCamera; y = Random.Range(-camHeight / 2, camHeight / 2); break;
            }

            Vector3 worldPos = new Vector3(x, y, 0) + cameraPos;
            Vector3Int cellPos = groundTilemap.WorldToCell(worldPos);

            // Check ground and collision tilemaps
            if (groundTilemap.HasTile(cellPos) && !collisionTilemap.HasTile(cellPos))
            {
                Vector3 center = groundTilemap.GetCellCenterWorld(cellPos);
                center.z = 0f; // Lock Z to 0 for all spawned enemies
                return center;
            }
        }

        Debug.LogWarning("Failed to find valid ground tile to spawn on.");
        return cameraPos;
    }
}
