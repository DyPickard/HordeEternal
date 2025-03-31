using UnityEngine;
using System.Collections;

public class ChargeAbility : MovementAbility
{
    [SerializeField] private float speedMultiplier = 1.25f; // 25% speed increase
    [SerializeField] private float chargeDuration = 3f;
    [SerializeField] private float particleEmissionRate = 30f;
    private bool isCharging = false;
    private ParticleSystem dustParticles;

    protected override void Start()
    {
        base.Start();
        cooldown = 10f;
        CreateDustParticleSystem();
    }

    private void CreateDustParticleSystem()
    {
        Debug.Log("Starting to create dust particle system...");

        GameObject dustObj = new GameObject("DustParticles");
        dustObj.transform.SetParent(transform);
        dustObj.transform.localPosition = new Vector3(0, -0.3f, 0);

        dustParticles = dustObj.AddComponent<ParticleSystem>();

        var main = dustParticles.main;
        main.loop = true;
        main.playOnAwake = false;
        main.duration = 1.0f;
        main.startLifetime = 0.5f;
        main.startSpeed = 0.5f;
        main.startSize = 0.2f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 50;
        main.gravityModifier = 0.1f;

        // Emission module
        var emission = dustParticles.emission;
        emission.rateOverTime = 0;
        emission.enabled = true;

        // Shape module
        var shape = dustParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.15f;
        shape.radiusThickness = 1.0f;

        // Color module
        var colorOverLifetime = dustParticles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.white, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(0.5f, 0.0f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        colorOverLifetime.color = gradient;

        // Renderer configuration
        var renderer = dustParticles.GetComponent<ParticleSystemRenderer>();
        renderer.sortingLayerName = "Default";
        renderer.sortingOrder = 1;
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));

        dustParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        Debug.Log("Dust particle system created successfully");
    }

    public override void UseAbility()
    {
        if (!CanUseAbility() || isCharging) return;

        StartCoroutine(PerformCharge());
        StartCooldown();
    }

    private IEnumerator PerformCharge()
    {
        Debug.Log("Starting charge boost!");
        isCharging = true;

        float originalSpeed = playerMovement.speed;
        playerMovement.speed *= speedMultiplier; // Multiply by 1.25 for 25% increase

        if (dustParticles != null)
        {
            var emission = dustParticles.emission;
            emission.rateOverTime = particleEmissionRate;
            dustParticles.Play();
            Debug.Log("Dust particles started");
        }

        yield return new WaitForSeconds(chargeDuration);

        // Restore original speed
        playerMovement.speed = originalSpeed;

        // Handle particles
        if (dustParticles != null)
        {
            dustParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            Debug.Log("Dust particles stopped");
        }

        isCharging = false;
        Debug.Log("Charge boost ended!");
    }

    private void OnDestroy()
    {
        if (dustParticles != null)
        {
            Destroy(dustParticles.gameObject);
        }
    }
}