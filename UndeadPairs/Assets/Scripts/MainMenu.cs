using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Interactions")]
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip hover;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PracticeStart()
    {
        PlayClick();
        SceneManager.LoadScene("Practice");
    }

    public void EasyStart()
    {
        PlayClick();
        SceneManager.LoadScene("Easy");
    }

    public void NormalStart()
    {
        PlayClick();
        SceneManager.LoadScene("Normal");
    }

    public void DifficultStart()
    {
        PlayClick();
        SceneManager.LoadScene("Hard");
    }

    /* public void Quit()
    {
        Application.Quit();
    } */

    private void PlayClick()
    {
        audioSource.PlayOneShot(click);
    }

    public void PlayHover()
    {
        audioSource.PlayOneShot(hover);
    }
}
