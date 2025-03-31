using UnityEngine;

public class Lightning_Bolt : MonoBehaviour
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
        damage = playerLevel.level * damage;
        float firerate = Mathf.Max(2f, baserate - (2*(playerLevel.level)));

        timer += Time.deltaTime;
        if (timer > firerate)
        {
            timer = 0;
            Activate();
        }
    }
    public void Activate()
    {
        Debug.Log("All batteries fire, fire!");
        Instantiate(lightning_proj, firePosition.position, firePosition.rotation);

    }
}

