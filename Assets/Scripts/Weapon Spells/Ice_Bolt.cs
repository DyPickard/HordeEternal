using UnityEngine;

public class Ice_Bolt : WeaponSpell
{
    public PlayerLevel playerLevel;

    public int baserate = 3;
    public int damage = 1;
    public int size = 1;

    public float timer = 0;

    public Transform firePosition;
    public GameObject ice_proj;

    void Update()
    {
        damage = playerLevel.level * damage;
        float firerate = Mathf.Max(0.5f, baserate - (0.5f * (playerLevel.level)));

        timer += Time.deltaTime;
        if (timer > firerate)
        {
            timer = 0;
            Activate();
        }
    }
    public override void Activate()
    {
        Debug.Log("All batteries fire, fire!");
        Instantiate(ice_proj, firePosition.position, firePosition.rotation);
    }
}

