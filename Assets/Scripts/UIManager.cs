using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Slider healthBar;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;

    private int score = 0;

    void Start()
    {
        UpdateScore(0);
        UpdateHealth(100);
        UpdateLevel(1);
    }

    public void UpdateScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }

    public void UpdateHealth(float value)
    {
        healthBar.value = value;
    }

    public void UpdateLevel(int level)
    {
        levelText.text = "Level: " + level;
    }
}
