using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadManager : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
