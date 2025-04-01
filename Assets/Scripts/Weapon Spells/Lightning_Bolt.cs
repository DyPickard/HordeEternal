using UnityEngine;

public class Lightning_Bolt : WeaponSpell
{
    public PlayerLevel playerLevel;
    [SerializeField] private AudioClip lightningSound;

    public int baserate = 10;
    public int damage = 1;
    public int targets = 1;

    public float timer = 0;

    public Transform firePosition;
    public GameObject lightning_proj;

    void Update()
    {
        float firerate = Mathf.Max(2f, baserate - (2*(playerLevel.level)));

        timer += Time.deltaTime;
        if (timer > firerate)
        {
            timer = 0;
            Activate();
        }
    }

    public int GetDamage()
    {
        return damage * playerLevel.level;
    }

    public void SetSound(AudioClip clip)
    {
        lightningSound = clip;
    }

    public override void Activate()
    {
        GameObject go = Instantiate(lightning_proj, firePosition.position, firePosition.rotation);
        AudioManager.Instance.PlaySFX(lightningSound);

        Bounce_Behave bounce = go.GetComponent<Bounce_Behave>();
        if (bounce != null)
        {
            bounce.damage = GetDamage();
        }
    }
}

