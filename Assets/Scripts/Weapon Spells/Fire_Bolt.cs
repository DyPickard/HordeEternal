using UnityEngine;

public class Fire_Bolt : WeaponSpell
{
    public PlayerLevel playerLevel;

    public int baserate = 5;
    public int damage = 2;
    public int size = 1;

    public float timer = 0;

    public Transform firePosition;
    public GameObject proj;

    void Update()
    {
        damage = playerLevel.level * damage;
        float firerate = Mathf.Max(1f, baserate - (1 * (playerLevel.level)));

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
        Instantiate(proj, firePosition.position, firePosition.rotation);
    }
}
