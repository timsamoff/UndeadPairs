using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private Toggle cardSfxToggle;
    [SerializeField] private Toggle otherSfxToggle;
    [SerializeField] private Toggle musicToggle;

    [SerializeField] private GameObject cardSfxParentObject;
    [SerializeField] private AudioSource[] otherSfx;
    [SerializeField] private AudioSource music;

    private AudioSource[] cardSfx;

    private void Start()
    {
        if (cardSfxParentObject != null)
        {
            cardSfx = cardSfxParentObject.GetComponentsInChildren<AudioSource>();
        }

        if (cardSfxToggle != null)
        {
            cardSfxToggle.onValueChanged.AddListener(OnCardSfxToggleValueChanged);
            UpdateAudioState(cardSfx, cardSfxToggle.isOn);
        }

        if (otherSfxToggle != null)
        {
            otherSfxToggle.onValueChanged.AddListener(OnOtherSfxToggleValueChanged);
            UpdateAudioState(otherSfx, otherSfxToggle.isOn);
        }

        if (musicToggle != null)
        {
            musicToggle.onValueChanged.AddListener(OnMusicToggleValueChanged);
            UpdateMusicState(musicToggle.isOn);
        }
    }

    private void OnCardSfxToggleValueChanged(bool isOn)
    {
        UpdateAudioState(cardSfx, isOn);
    }

    private void OnOtherSfxToggleValueChanged(bool isOn)
    {
        UpdateAudioState(otherSfx, isOn);
    }

    private void OnMusicToggleValueChanged(bool isOn)
    {
        UpdateMusicState(isOn);
    }

    private void UpdateAudioState(AudioSource[] audioSources, bool isAudioOn)
    {
        if (audioSources != null)
        {
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource != null)
                {
                    audioSource.mute = !isAudioOn; // Mute/unmute audio
                }
            }
        }
    }

    private void UpdateMusicState(bool isMusicOn)
    {
        if (music != null)
        {
            music.mute = !isMusicOn; // Mute/unmute music
        }
    }
}