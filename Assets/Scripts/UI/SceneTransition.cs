using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 1f;

    private void Start()
    {
        fadePanel.gameObject.SetActive(true);
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn()
    {
        fadePanel.gameObject.SetActive(true);

        float elapsedTime = 0f;
        Color color = fadePanel.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            fadePanel.color = color;
            yield return null;
        }

        fadePanel.gameObject.SetActive(false);
    }

    IEnumerator FadeOut(string sceneName)
    {
        fadePanel.gameObject.SetActive(true);

        float elapsedTime = 0f;
        Color color = fadePanel.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
        StartCoroutine(FadeIn());
    }
}
