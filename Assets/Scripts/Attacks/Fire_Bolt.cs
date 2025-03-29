using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Fire_Bolt : MonoBehaviour
{
    public PlayerLevel playerLevel;

    public int level = 1;
    public int baserate = 5;
    public int damage = 1;
    public int size = 1;

    public float timer = 0;

    public Transform firePosition;
    public GameObject proj;

    private void Start()
    {
        playerLevel.LevelUp.AddListener(() => level += 1); 
    }
    void Update()
    {
        int firerate = baserate - level;

        timer += Time.deltaTime;
        if (timer > firerate)
        {
            timer = 0;
            Fire();
        }
    }
    private void Fire()
    {
        Debug.Log("All batteries fire, fire!");
        Instantiate(proj, firePosition.position, firePosition.rotation);
    }

}
