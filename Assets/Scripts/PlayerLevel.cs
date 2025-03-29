using UnityEngine;
using UnityEngine.Events;

public class PlayerLevel : MonoBehaviour
{
    //[SerializeField] private TMPro.TextMeshProUGUI levelText;
    [SerializeField] private int level = 1;
    [SerializeField] private int exp = 0;
    [SerializeField] private int nextLevelExp = 10;

    // Added LevelUp event so that all listeners in the game know that the player leveled up in real time
    public UnityEvent LevelUp;

    public void Update()
    {
        if (exp > nextLevelExp)
        {
            IncreaseLevel();
        }
    }

    public void IncreaseLevel()
    {
        nextLevelExp = nextLevelExp * 2;

        // Invoke leveling up
        LevelUp.Invoke();
    }
    public void IncreaseExp(int e) {
        exp += e;   
    }
}
