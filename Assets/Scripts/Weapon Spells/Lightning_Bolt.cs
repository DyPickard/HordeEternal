using UnityEngine;

public class Lightning_Bolt : WeaponSpell
{
    public PlayerLevel playerLevel;

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

    public override void Activate()
    {
        GameObject go = Instantiate(lightning_proj, firePosition.position, firePosition.rotation);

        Bolt_Behavior bolt = go.GetComponent<Bolt_Behavior>();
        if (bolt != null)
        {
            bolt.damage = GetDamage();
        }
    }
}

