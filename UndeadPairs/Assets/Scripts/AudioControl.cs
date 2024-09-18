using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public static AudioControl Instance { get; private set; }

    [Header("Audio Settings")]
    [SerializeField] private GameObject cardSfxParentObject; // Parent object containing card SFX
    [SerializeField] private AudioSource[] otherSfx;         // Array for other SFX
    [SerializeField] private AudioSource music;              // Music AudioSource

    private AudioSource[] cardSfx; // Array to hold card SFX AudioSources

    private bool isSfxMuted = false;

    private void Awake()
    {
        // Singleton pattern setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Get all the card SFX sources from the parent object
        if (cardSfxParentObject != null)
        {
            cardSfx = cardSfxParentObject.GetComponentsInChildren<AudioSource>();
        }

        // Optionally, you can load the saved states from PlayerPrefs here and apply them:
        bool sfxEnabled = PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1;
        UpdateAudioState(cardSfx, sfxEnabled);
        UpdateAudioState(otherSfx, sfxEnabled);

        bool musicEnabled = PlayerPrefs.GetInt("Music_Toggle_State", 1) == 1;
        UpdateMusicState(musicEnabled);
    }

    // Public method to handle SFX toggle (controls both card and other SFX)
    public void OnSfxToggleValueChanged(bool isOn)
    {
        isSfxMuted = !isOn;
        UpdateAudioState(cardSfx, isOn);  // Control card SFX
        UpdateAudioState(otherSfx, isOn); // Control other SFX
        PlayerPrefs.SetInt("SFX_Toggle_State", isOn ? 1 : 0); // Save toggle state
    }

    // Public method to handle Music toggle
    public void OnMusicToggleValueChanged(bool isOn)
    {
        UpdateMusicState(isOn);
        PlayerPrefs.SetInt("Music_Toggle_State", isOn ? 1 : 0); // Save toggle state
    }

    public bool IsSfxMuted()
    {
        return isSfxMuted;
    }

    // Helper method to update the mute state of AudioSource arrays
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

    // Helper method to update the music mute state
    private void UpdateMusicState(bool isMusicOn)
    {
        if (music != null)
        {
            music.mute = !isMusicOn; // Mute/unmute music
        }
    }
}