using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public FadeToBlack fadeToBlack;

    private static bool gameLaunched = false;

    [SerializeField] private BackgroundMusic backgroundMusic;

    void Start()
    {
        fadeToBlack.onFadeComplete += LoadScene;

        if (!gameLaunched)
        {
            ResetPlayerPrefs();
            gameLaunched = true; // Set to true after the initial reset
        }
    }

    private void ResetPlayerPrefs()
    {
        // Reset the PlayerPrefs for SFX and Music toggles
        PlayerPrefs.SetInt("SFX_Toggle_State", 1);
        PlayerPrefs.SetInt("Music_Toggle_State", 1);
        PlayerPrefs.Save();
    }

    private string sceneToLoad;

    public void PracticeStart()
    {
        sceneToLoad = "Practice";
        StartSceneTransition();
    }

    public void EasyStart()
    {
        sceneToLoad = "Easy";
        StartSceneTransition();
    }

    public void NormalStart()
    {
        sceneToLoad = "Normal";
        StartSceneTransition();
    }

    public void DifficultStart()
    {
        sceneToLoad = "Difficult";
        StartSceneTransition();
    }

    public void About()
    {
        sceneToLoad = "About";
        StartSceneTransition();
    }

    private void StartSceneTransition()
    {
        StartCoroutine(FadeOutAndTransition());
    }

    private IEnumerator FadeOutAndTransition()
    {
        // Start fading out the music
        yield return StartCoroutine(backgroundMusic.FadeOutMusic());

        fadeToBlack.StartFade();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnDestroy()
    {
        fadeToBlack.onFadeComplete -= LoadScene;
    }
}