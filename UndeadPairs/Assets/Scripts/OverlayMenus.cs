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
            Debug.Log("PauseScreen script not found!");
        }
    }

    public void ResumeGame()
    {
        if (pauseScreen != null)
        {
            StartCoroutine(FadeOutAndResumeGame());
            pauseScreen.OnResumeButtonPressed();
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene("MainMenu");

        UnloadSceneIfLoaded("Pause");
        UnloadSceneIfLoaded("Lose");
        UnloadSceneIfLoaded("Win");
    }

    // Coroutine to handle fade out and resume game
    private IEnumerator FadeOutAndResumeGame()
    {
        pauseScreen.OnResumeButtonPressed();

        yield return new WaitForSecondsRealtime(pauseScreen.FadeTime); // Wait for fade to finish

        UnloadSceneIfLoaded("Pause");
    }

    private void UnloadSceneIfLoaded(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}