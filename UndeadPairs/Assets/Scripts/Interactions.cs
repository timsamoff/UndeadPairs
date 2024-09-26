using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactions : MonoBehaviour
{
    [Header("Interactions")]
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip hover;
    [SerializeField] private AudioSource audioSource;

    private bool soundOn = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SoundOn(bool enabled)
    {
        soundOn = enabled;
    }

    public  void PlayClick()
    {
        if (PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1 && soundOn)
        {
            audioSource.PlayOneShot(click);
        }
    }

    public void PlayHover()
    {
        if (PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1 && soundOn)
        {
            audioSource.PlayOneShot(hover);
        }
    }
}
