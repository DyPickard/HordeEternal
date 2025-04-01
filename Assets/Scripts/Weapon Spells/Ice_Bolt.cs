using UnityEngine;

public class Ice_Bolt : WeaponSpell
{
    public PlayerLevel playerLevel;
    [SerializeField] private AudioClip iceSound;

    public int baserate = 3;
    public int damage = 1;
    public int size = 1;

    public float timer = 0;

    public Transform firePosition;
    public GameObject ice_proj;

    void Update()
    {
        float firerate = Mathf.Max(0.5f, baserate - (0.5f * (playerLevel.level)));

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
        iceSound = clip;
    }

    public override void Activate()
    {
        GameObject go = Instantiate(ice_proj, firePosition.position, firePosition.rotation);
        AudioManager.Instance.PlaySFX(iceSound);

        Bolt_Behavior bolt = go.GetComponent<Bolt_Behavior>();
        if (bolt != null)
        {
            bolt.damage = GetDamage();
        }
    }
}