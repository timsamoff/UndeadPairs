using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverlayMenus : MonoBehaviour
{
    private PauseScreen pauseScreen;

    private void Start()
    {
        pauseScreen = FindObjectOfType<PauseScreen>();

        if (pauseScreen == null)
        {
            Debug.LogError("PauseScreen script not found!");
        }
    }

    public void ResumeGame()
    {
        if (pauseScreen != null)
        {
            StartCoroutine(FadeOutAndResumeGame());
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
        SceneManager.UnloadSceneAsync("Pause");
    }

    private IEnumerator FadeOutAndResumeGame()
    {
        pauseScreen.OnResumeButtonPressed();

        yield return new WaitForSecondsRealtime(pauseScreen.FadeTime);

        SceneManager.UnloadSceneAsync("Pause");
    }
}