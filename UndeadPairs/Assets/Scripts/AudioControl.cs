using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public static AudioControl Instance { get; private set; }

    [Header("Audio Settings")]
    [SerializeField] private GameObject cardSfxParentObject;
    [SerializeField] private AudioSource[] otherSfx;
    [SerializeField] private AudioSource music;

    private AudioSource[] cardSfx;
    private bool isSfxMuted = false;

    private void Awake()
    {
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
        if (cardSfxParentObject != null)
        {
            cardSfx = cardSfxParentObject.GetComponentsInChildren<AudioSource>();
        }

        LoadAudioSettings();
    }

    private void LoadAudioSettings()
    {
        bool sfxEnabled = PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1;
        UpdateAudioState(sfxEnabled);
        bool musicEnabled = PlayerPrefs.GetInt("Music_Toggle_State", 1) == 1;
        UpdateMusicState(musicEnabled);
    }

    public void OnSfxToggleValueChanged(bool isOn)
    {
        isSfxMuted = !isOn;
        UpdateAudioState(isOn);
        PlayerPrefs.SetInt("SFX_Toggle_State", isOn ? 1 : 0);
    }

    public void OnMusicToggleValueChanged(bool isOn)
    {
        UpdateMusicState(isOn);
        PlayerPrefs.SetInt("Music_Toggle_State", isOn ? 1 : 0);
    }

    public bool IsSfxMuted()
    {
        return isSfxMuted;
    }

    private void UpdateAudioState(bool isAudioOn)
    {
        SetMute(cardSfx, isAudioOn);
        SetMute(otherSfx, isAudioOn);
    }

    private void SetMute(AudioSource[] audioSources, bool isAudioOn)
    {
        if (audioSources != null)
        {
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource != null)
                {
                    audioSource.mute = !isAudioOn;
                }
            }
        }
    }

    private void UpdateMusicState(bool isMusicOn)
    {
        if (music != null)
        {
            music.mute = !isMusicOn;
        }
    }

    public void PlayClipAtPosition(AudioClip clip, Vector3 position)
    {
        if (isSfxMuted || clip == null)
        {
            Debug.Log("Audio muted or clip is null, not playing audio.");
            return;
        }

        AudioSource.PlayClipAtPoint(clip, position);
        Debug.Log("Playing audio clip at position: " + position);
    }
}