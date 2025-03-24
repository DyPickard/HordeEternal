using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnDistanceFromCamera = 2f;

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
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 cameraPos = mainCamera.transform.position;
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        // Expand the bounds by spawnDistanceFromCamera
        float x = 0f, y = 0f;
        int side = Random.Range(0, 4); // 0 = Top, 1 = Bottom, 2 = Left, 3 = Right

        switch (side)
        {
            case 0: // Top
                x = Random.Range(-camWidth / 2, camWidth / 2);
                y = camHeight / 2 + spawnDistanceFromCamera;
                break;
            case 1: // Bottom
                x = Random.Range(-camWidth / 2, camWidth / 2);
                y = -camHeight / 2 - spawnDistanceFromCamera;
                break;
            case 2: // Left
                x = -camWidth / 2 - spawnDistanceFromCamera;
                y = Random.Range(-camHeight / 2, camHeight / 2);
                break;
            case 3: // Right
                x = camWidth / 2 + spawnDistanceFromCamera;
                y = Random.Range(-camHeight / 2, camHeight / 2);
                break;
        }

        // Convert local offset to world position relative to camera
        return new Vector3(x, y, 0) + cameraPos;
    }
}
