using UnityEngine;
using UnityEngine.Events;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI levelText;
    [SerializeField] private UnityEngine.UI.Image expBar;

    public int level = 1;
    [SerializeField] private int exp = 0;
    [SerializeField] private int nextLevelExp = 10;

    public UnityEvent LevelUp = new UnityEvent();

    private UIManager uiManager;

    public int Level
    {
        get => level;
        set
        {
            level = value;
            UpdateLevelUI();
        }
    }

    public int Exp
    {
        get => exp;
        set
        {
            exp = value;
            CheckLevelUp(); // Auto-check level up when exp changes
            UpdateLevelUI();
        }
    }

    public int NextLevelExp
    {
        get => nextLevelExp;
        set
        {
            nextLevelExp = value;
            UpdateLevelUI();
        }
    }

    private void Start()
    {
        InitializeUI();
    }

    // Update UI when changing values in the Inspector during play mode
    private void OnValidate()
    {
        CheckLevelUp();
        UpdateLevelUI();
    }

    public void InitializeUI(UIManager manager = null)
    {
        if (manager == null)
        {
            uiManager = UnityEngine.Object.FindAnyObjectByType<UIManager>();
        }
        else
        {
            uiManager = manager;
        }

        if (uiManager != null)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        levelText = uiManager.levelText;
        expBar = uiManager.expBar;

        UpdateLevelUI();
    }

    private void CheckLevelUp()
    {
        while (exp >= nextLevelExp)
        {
            exp -= nextLevelExp;
            Level++; // This will call the setter and update UI
            nextLevelExp *= 2;
        }
    }

    public void IncreaseExp(int e)
    {
        Exp += e; // Use the setter to update exp and trigger UI update
    }

    private void UpdateLevelUI()
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + level;
        }

        if (expBar != null)
        {
            expBar.fillAmount = (float)exp / nextLevelExp;
        }
    }
}