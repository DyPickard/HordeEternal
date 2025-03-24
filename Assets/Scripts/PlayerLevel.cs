using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI levelText;
    private int level = 1;
    private int exp = 0;
    private int nextLevelExp = 5;


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
    }
    public void IncreaseExp(int e) {
        exp += e;   
    }
}
