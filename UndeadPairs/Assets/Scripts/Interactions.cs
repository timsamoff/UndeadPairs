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
        audioSource.PlayOneShot(click);
    }

    public void PlayHover()
    {
        Debug.Log(audioSource);
        audioSource.PlayOneShot(hover);
    }
}
