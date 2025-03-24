using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject mainMenuPanel;

    [Header("Lives")]
    public GameObject heartPrefab;
    public Transform livesPanel;
    private int maxLives = 5;
    private GameObject[] hearts;

    [Header("Power-Ups")]
    public Image weaponBox;
    public Image utilityBox;
    public Sprite emptySlotSprite;

    [Header("Score")]
    public TextMeshProUGUI scoreText;
    private int score = 0;

    void Start()
    {
        InitLives(maxLives);
        UpdateScore(0);
        ClearPowerUps();
    }

    public void ShowGameUI()
    {
        gameUI.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        gameUI.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    void InitLives(int lives)
    {
        hearts = new GameObject[lives];
        for (int i = 0; i < lives; i++)
        {
            hearts[i] = Instantiate(heartPrefab, livesPanel);
        }
    }

    public void UpdateLives(int currentLives)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentLives);
        }
    }

    public void UpdateScore(int amount)
    {
        score += amount;
        TextMeshProUGUI scoreText = gameUI.GetComponentInChildren<TextMeshProUGUI>();
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void SetWeaponPowerUp(Sprite weaponSprite)
    {
        weaponBox.sprite = weaponSprite;
    }

    public void SetUtilityPowerUp(Sprite utilitySprite)
    {
        utilityBox.sprite = utilitySprite;
    }

    public void ClearPowerUps()
    {
        weaponBox.sprite = emptySlotSprite;
        utilityBox.sprite = emptySlotSprite;
    }
}
