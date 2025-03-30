using UnityEngine;
using TMPro;

public class GameClock : MonoBehaviour
{
    public static GameClock Instance { get; private set; }
    public int gameTime = 0; // Timer in seconds
    public bool isGameRunning = false;
    private float secondCounter = 0f;
    private TextMeshProUGUI timeText;

    // singleton pattern
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // prevent duplicates
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // optional
    }
    private void Start()
    {
        // find the TextMeshProUGUI component for time text
        timeText = GameObject.Find("TimeDisplay").GetComponent<TextMeshProUGUI>();
        if (timeText == null)
        {
            Debug.LogError("TimeDisplay TextMeshProUGUI not found!");
        }
    }
    void Update()
    {
        if (isGameRunning)
        {
            secondCounter += Time.deltaTime;
        }
        if (secondCounter >= 1f)
        {
            gameTime++;
            DisplayTime(gameTime);
            secondCounter = 0f;
        }
    }

    void DisplayTime(int timeInSeconds)
    {
        int minutes = (timeInSeconds / 60);
        int seconds = (timeInSeconds % 60);
        //Debug.Log(string.Format("{0:00}:{1:00}", minutes, seconds));
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void PauseTime()
    {
        isGameRunning = false;
    }
    public void ResumeTime()
    {
        isGameRunning = true;
    }

    public void StartGame()
    {
        gameTime = 0;
        secondCounter = 0f;
        isGameRunning = true;
    }

    public void EndGame()
    {
        gameTime = 0;
        secondCounter = 0f;
        isGameRunning = false;
    }
}
