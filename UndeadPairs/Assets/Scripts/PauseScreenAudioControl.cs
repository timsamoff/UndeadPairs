using UnityEngine;
using UnityEngine.UI;

public class PauseScreenAudioControl : MonoBehaviour
{
    [SerializeField] private Toggle sfxToggle;   // Single toggle for both card SFX and other SFX
    [SerializeField] private Toggle musicToggle; // Separate toggle for music

    private void Start()
    {
        if (AudioControl.Instance != null)
        {
            // Load saved SFX and music toggle states
            sfxToggle.isOn = PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1;
            musicToggle.isOn = PlayerPrefs.GetInt("Music_Toggle_State", 1) == 1;

            // Add listeners to update the audio settings when toggles are changed
            sfxToggle.onValueChanged.AddListener(AudioControl.Instance.OnSfxToggleValueChanged);
            musicToggle.onValueChanged.AddListener(AudioControl.Instance.OnMusicToggleValueChanged);
        }
    }

    private void OnDestroy()
    {
        // Remove listeners to avoid memory leaks
        if (sfxToggle != null)
            sfxToggle.onValueChanged.RemoveListener(AudioControl.Instance.OnSfxToggleValueChanged);

        if (musicToggle != null)
            musicToggle.onValueChanged.RemoveListener(AudioControl.Instance.OnMusicToggleValueChanged);
    }
}