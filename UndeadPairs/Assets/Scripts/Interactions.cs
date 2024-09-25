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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public  void PlayClick()
    {
        if (PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1)
        {
            audioSource.PlayOneShot(click);
        }
    }

    public void PlayHover()
    {
        if (PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1)
        {
            audioSource.PlayOneShot(hover);
        }
    }
}
