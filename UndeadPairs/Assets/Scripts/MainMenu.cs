using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public FadeToBlack fadeToBlack;

    void Start()
    {
        fadeToBlack.onFadeComplete += LoadScene;
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