using UnityEngine;

public class Fire_Bolt : WeaponSpell
{
    public PlayerLevel playerLevel;

    [SerializeField] private int baseFireRate = 5;
    public int baserate { get { return baseFireRate; } set { baseFireRate = value; } }
    public int damage = 1;
    public int size = 1;

    public float timer = 0;

    public Transform firePosition;
    public GameObject proj;

    void Update()
    {
        int firerate = Mathf.Max(1, baseFireRate - playerLevel.level);

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

    public int GetFireRate()
    {
        return baseFireRate;
    }

    public void SetFireRate(int newRate)
    {
        baseFireRate = Mathf.Max(1, newRate);
    }
}
