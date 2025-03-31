using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject skeletonPrefab;
    [SerializeField] private GameObject ogrePrefab;
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private GameObject dragonPrefab;

    [Header("Spawn Settings")]
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
    [SerializeField][Range(0, 100)] private float dragonSpawnWeight = 5f;
    [SerializeField] private GameClock gameClock;
    private float timer;

    void Update()
    {
        TimerSpawnIntervalChange(gameClock.gameTime);

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
        float totalWeight = skeletonSpawnWeight + ogreSpawnWeight + ghostSpawnWeight + dragonSpawnWeight;
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
        else if (randomValue <= skeletonSpawnWeight + ogreSpawnWeight + ghostSpawnWeight)
        {
            enemyToSpawn = ghostPrefab;
            spawnPos = GetRandomGhostSpawnPosition();
        }
        else
        {
            enemyToSpawn = dragonPrefab;
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

    void TimerSpawnIntervalChange(int seconds)
    {
        switch (seconds)
        {
            case < 60:
                spawnInterval = 2f;
                break;
            case < 120:
                spawnInterval = 1.8f;
                break;
            case < 180:
                spawnInterval = 1.6f;
                break;
            case < 240:
                spawnInterval = 1.4f;
                break;
            case < 300:
                spawnInterval = 1.2f;
                break;
            default:
                break;
        }
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
