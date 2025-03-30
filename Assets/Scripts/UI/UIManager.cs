using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject pauseUI;
    public GameObject gameOverUI;
    public Button resumeButton;
    public Button quitButton;
    public Button exitButton;
    public AudioClip buttonClickSFX;

    [Header("Lives")]
    public GameObject heartPrefab;
    public Transform livesPanel;
    private int maxLives = 6;
    private GameObject[] hearts;

    [Header("Power-Ups")]
    public Image weaponBox;
    public Image utilityBox;
    public Sprite emptySlotSprite;

    [Header("Level")]
    public TextMeshProUGUI levelText;
    public UnityEngine.UI.Image expBar;

    private bool isPaused = false;

    void Start()
    {
        InitLives(maxLives);
        ClearPowerUps();

        resumeButton.onClick.AddListener(() => ButtonClick(ResumeGame));
        quitButton.onClick.AddListener(() => ButtonClick(ReturnToTitle));
        exitButton.onClick.AddListener(() => ButtonClick(ExitGame));
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

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseUI.SetActive(false);
    }

    public void ReturnToTitle()
    {
        AudioManager.Instance.StopMusic();
        Time.timeScale = 1f;
        gameUI.SetActive(false);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene("TitleScreen");
    }

    public void ExitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    void ButtonClick(System.Action action)
    {
        if (buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(buttonClickSFX);
        }
        action?.Invoke();
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

    public void SetWeaponPowerUp(Sprite weaponSprite)
    {
        Debug.Log("Setting weapon icon to: " + weaponSprite?.name);
        weaponBox.sprite = weaponSprite;

        // Ensure icon is visible
        weaponBox.color = Color.white;
    }

    public void SetUtilityPowerUp(Sprite utilitySprite)
    {
        Debug.Log("Setting weapon icon to: " + utilitySprite?.name);
        utilityBox.sprite = utilitySprite;

        if (utilitySprite == null)
        {
            // Set full transparency
            Color transparent = new Color(1f, 1f, 1f, 0f);
            utilityBox.color = transparent;
        } else {
            // Ensure icon is visible
            utilityBox.color = Color.white;
        }
    }

    public void ClearPowerUps()
    {
        weaponBox.sprite = null;
        utilityBox.sprite = null;

        // Set full transparency
        Color transparent = new Color(1f, 1f, 1f, 0f);
        weaponBox.color = transparent;
        utilityBox.color = transparent;
    }
}
