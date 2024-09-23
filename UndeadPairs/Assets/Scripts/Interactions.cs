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

    private AudioControl audioControl;

    void Start()
    {
        audioControl = FindAnyObjectByType<AudioControl>();

        audioSource = GetComponent<AudioSource>();
    }

    public  void PlayClick()
    {
        if (!audioControl.IsSfxMuted())
        {
            audioSource.PlayOneShot(click);
        }
    }

    public void PlayHover()
    {
        if (!audioControl.IsSfxMuted())
        {
            audioSource.PlayOneShot(hover);
        }
    }
}
