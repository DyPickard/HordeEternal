using UnityEngine;
using System.Collections;

public class Shockwave : UtilitySpell
{
    [Header("Wave Properties")]
    [SerializeField] private float maxRadius = 15f;
    [SerializeField] private float expansionSpeed = 10f;
    [SerializeField] private float damage = 2f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private Color waveColor = new Color(0.7f, 0.7f, 1f, 0.5f);

    [Header("Visual Effects")]
    [SerializeField] private float waveThickness = 0.5f;
    [SerializeField] private Material waveMaterial;

    private bool isActive = false;
    private LineRenderer waveRenderer;
    private Transform playerTransform;

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.Player != null)
        {
            playerTransform = GameManager.Instance.Player.transform;
            Debug.Log("Shockwave: Successfully found player transform");
        }
        else
        {
            Debug.LogError("Shockwave: Could not find player transform!");
        }

        GameObject waveObject = new GameObject("ShockwaveVisual");
        waveObject.transform.SetParent(transform);
        waveRenderer = waveObject.AddComponent<LineRenderer>();

        waveRenderer.startWidth = waveThickness;
        waveRenderer.endWidth = waveThickness;
        waveRenderer.positionCount = 360;
        waveRenderer.loop = true;
        waveRenderer.useWorldSpace = true;

        if (waveMaterial != null)
        {
            waveRenderer.material = waveMaterial;
        }
        else
        {
            waveRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        waveRenderer.startColor = waveColor;
        waveRenderer.endColor = waveColor;
        waveRenderer.enabled = false;
    }

    public override void Activate()
    {
        if (!isActive)
        {
            if (playerTransform == null && GameManager.Instance != null)
            {
                playerTransform = GameManager.Instance.Player?.transform;
            }

            if (playerTransform != null)
            {
                Debug.Log("Starting shockwave at player position: " + playerTransform.position);
                StartCoroutine(CreateShockwave());
            }
            else
            {
                Debug.LogError("Cannot create shockwave - player transform is null!");
            }
        }
    }

    private IEnumerator CreateShockwave()
    {
        isActive = true;
        float currentRadius = 0f;
        waveRenderer.enabled = true;
        Vector3 origin = playerTransform.position;

        while (currentRadius < maxRadius)
        {
            currentRadius += expansionSpeed * Time.deltaTime;

            UpdateWaveVisual(origin, currentRadius);

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(origin, currentRadius);
            foreach (Collider2D col in hitColliders)
            {
                Enemy enemy = col.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Vector2 direction = (col.transform.position - origin).normalized;
                    float distance = Vector2.Distance(origin, col.transform.position);

                    enemy.TakeDamage((int)damage);

                    Rigidbody2D enemyRb = col.GetComponent<Rigidbody2D>();
                    if (enemyRb != null)
                    {
                        float knockbackMultiplier = 1 - (distance / maxRadius);
                        enemyRb.AddForce(direction * knockbackForce * knockbackMultiplier, ForceMode2D.Impulse);
                    }
                }
            }

            yield return null;
        }

        waveRenderer.enabled = false;
        isActive = false;

        PlayerSpellManager spellManager = GetComponentInParent<PlayerSpellManager>();
        if (spellManager != null)
        {
            spellManager.ClearUtilitySpell();
        }
    }

    private void UpdateWaveVisual(Vector3 origin, float radius)
    {
        Vector3[] positions = new Vector3[360];
        for (int i = 0; i < 360; i++)
        {
            float angle = i * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            positions[i] = origin + new Vector3(x, y, 0);
        }
        waveRenderer.SetPositions(positions);
    }
}