using UnityEngine;

public class Fire_Bolt : WeaponSpell
{
    public PlayerLevel playerLevel;
    [SerializeField] private AudioClip fireballSound;

    public int baserate = 5;
    public int damage = 2;
    public int size = 1;

    public float timer = 0;

    public Transform firePosition;
    public GameObject proj;

    void Start()
    {
        fireballSound = Resources.Load<AudioClip>("Spells/FireBall");
    }

    void Update()
    {
        float firerate = Mathf.Max(1f, baserate - (1 * (playerLevel.level)));

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

    public override void Activate()
    {
        GameObject go = Instantiate(proj, firePosition.position, firePosition.rotation);
        AudioManager.Instance.PlaySFX(fireballSound);

        Bolt_Behavior bolt = go.GetComponent<Bolt_Behavior>();
        if (bolt != null)
        {
            bolt.damage = GetDamage();
        }
    }
}
