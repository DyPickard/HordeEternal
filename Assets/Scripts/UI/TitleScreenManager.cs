using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public AudioClip buttonClickSFX;
    public AudioClip backgroundMusic;

    void Start()
    {
        Invoke("PlayMusic", 0.5f);
    }

    public void StartGame()
    {
        PlayButtonClick();
        SceneManager.LoadScene("MainLevel");
    }

    public void QuitGame()
    {
        PlayButtonClick();
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    void PlayButtonClick()
    {
        if (buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(buttonClickSFX);
        }
    }

    void PlayMusic()
    {
        AudioManager.Instance.PlayMusic(backgroundMusic);
    }
}