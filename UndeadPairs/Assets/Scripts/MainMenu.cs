using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PracticeStart()
    {
        SceneManager.LoadScene("Practice");
    }

    public void EasyStart()
    {
        SceneManager.LoadScene("Easy");
    }

    public void NormalStart()
    {
        SceneManager.LoadScene("Normal");
    }

    public void HardStart()
    {
        SceneManager.LoadScene("Hard");
    }

    /* public void Quit()
    {
        Application.Quit();
    } */
}
