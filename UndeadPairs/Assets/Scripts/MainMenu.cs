using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public FadeToBlack fadeToBlack;

    private static bool gameLaunched = false;

    [SerializeField] private Button[] menuButtons;

    [SerializeField] private BackgroundMusic backgroundMusic;

    private InteractionSfx interactionSfx;

    void Start()
    {
        interactionSfx = GetComponent<InteractionSfx>();

        fadeToBlack.onFadeComplete += LoadScene;

        if (!gameLaunched)
        {
            ResetPlayerPrefs();
            gameLaunched = true; // Set to true after the initial reset
        }
    }

    private void ResetPlayerPrefs()
    {
        // Reset PlayerPrefs for SFX and Music toggles
        PlayerPrefs.SetInt("SFX_Toggle_State", 1);
        PlayerPrefs.SetInt("Music_Toggle_State", 1);
        PlayerPrefs.Save();
    }

    private string sceneToLoad;

    /*public void PracticeStart()
    {
        sceneToLoad = "Practice";
        DisableButtons();
        StartSceneTransition();
    }*/

    public void EasyStart()
    {
        sceneToLoad = "Easy";
        DisableButtons();
        StartSceneTransition();
    }

    public void NormalStart()
    {
        sceneToLoad = "Normal";
        DisableButtons();
        StartSceneTransition();
    }

    public void DifficultStart()
    {
        sceneToLoad = "Difficult";
        DisableButtons();
        StartSceneTransition();
    }

    public void About()
    {
        sceneToLoad = "About";
        DisableButtons();
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

    private void DisableButtons()
    {
        foreach (Button button in menuButtons)
        {
            button.interactable = false;
        }

        interactionSfx.SoundOn(false);
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