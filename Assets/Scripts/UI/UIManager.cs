using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;

    private int score = 0;

    void Start()
    {
        UpdateScore(0);
        UpdateLevel(1);
    }

    public void UpdateScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }

    public void UpdateLevel(int level)
    {
        levelText.text = "Level: " + level;
    }
}
