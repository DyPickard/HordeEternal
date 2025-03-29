using UnityEngine;
using UnityEngine.Events;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI levelText;
    private int level = 1;
    private int exp = 0;
    private int nextLevelExp = 10;

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
        level++;
        levelText.text = "Level: " + level;
        nextLevelExp = nextLevelExp * 2;

        // Invoke leveling up
        LevelUp.Invoke();
    }
    public void IncreaseExp(int e) {
        exp += e;   
    }
}
