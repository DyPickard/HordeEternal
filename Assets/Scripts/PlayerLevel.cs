using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    //[SerializeField] private TMPro.TextMeshProUGUI levelText;
    [SerializeField] private int level = 1;
    [SerializeField] private int exp = 0;
    [SerializeField] private int nextLevelExp = 10;


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
        level++;
        //levelText.text = "Level: " + level;
        
    }
    public void IncreaseExp(int e) {
        exp += e;   
    }
}
