using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    private SceneTransition sceneTransition;

    private void Start()
    {
        GameObject uiManager = GameObject.FindWithTag("UIManager");

        if (uiManager != null)
        {
            sceneTransition = uiManager.GetComponent<SceneTransition>();
        }
        else
        {
            Debug.LogError("UIManager/SceneTransition not found!");
        }
    }

    public void StartGame()
    {
        if (sceneTransition != null)
        {
            sceneTransition.FadeToScene("MainLevel");
        }
        else
        {
            Debug.LogError("SceneTransition not set.");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
