using UnityEngine;

public class Fire_Bolt : WeaponSpell
{
    public PlayerLevel playerLevel;
    [SerializeField] private AudioClip fireball;

    public int baserate = 5;
    public int damage = 1;
    public int size = 1;

    public float timer = 0;

    public Transform firePosition;
    public GameObject proj;

    void Update()
    {
        int firerate = Mathf.Max(1, baserate - playerLevel.level);

        timer += Time.deltaTime;
        if (timer > firerate)
        {
            timer = 0;
            Activate();
        }
    }

    public void SetFireballClip(AudioClip clip)
    {
        fireball = clip;
    }

    public override void Activate()
    {
        Debug.Log("All batteries fire, fire!");
        AudioManager.Instance.PlaySFX(fireball);
        Instantiate(proj, firePosition.position, firePosition.rotation);
    }
}
