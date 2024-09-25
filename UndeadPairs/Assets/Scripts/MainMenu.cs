using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public FadeToBlack fadeToBlack;

    private static bool gameLaunched = false;

    void Start()
    {
        fadeToBlack.onFadeComplete += LoadScene;

        if (!gameLaunched)
        {
            ResetPlayerPrefs();
            gameLaunched = true; // Set the flag to true after the initial reset
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
        fadeToBlack.StartFade();
    }

    public void EasyStart()
    {
        fadeToBlack.StartFade();
    }

    public void NormalStart()
    {
        sceneToLoad = "Normal";
        fadeToBlack.StartFade();
    }

    public void DifficultStart()
    {
        sceneToLoad = "Difficult";
        fadeToBlack.StartFade();
    }

    public void About()
    {
        SceneManager.LoadScene("About");
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