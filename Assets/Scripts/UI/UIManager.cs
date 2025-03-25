using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject pauseUI;
    public Button resumeButton;
    public Button quitButton;
    public Button exitButton;

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

    private bool isPaused = false;

    void Start()
    {
        InitLives(maxLives);
        UpdateScore(0);
        ClearPowerUps();

        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(ReturnToTitle);
        exitButton.onClick.AddListener(ExitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainLevel")
        {
            gameUI.SetActive(true);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseUI.SetActive(false);
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        gameUI.SetActive(false);
        SceneManager.LoadScene("TitleScreen");
    }

    public void ExitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
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
