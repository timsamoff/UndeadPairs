using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public static AudioControl Instance { get; private set; }

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
        LoadAudioSettings();
    }

    private void LoadAudioSettings()
    {
        bool sfxEnabled = PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1;
        bool musicEnabled = PlayerPrefs.GetInt("Music_Toggle_State", 1) == 1;
    }

    public void OnSfxToggleValueChanged(bool isOn)
    {
        isSfxMuted = !isOn;
        PlayerPrefs.SetInt("SFX_Toggle_State", isOn ? 1 : 0);
    }

    public void OnMusicToggleValueChanged(bool isOn)
    {
        PlayerPrefs.SetInt("Music_Toggle_State", isOn ? 1 : 0);
    }

    public bool IsSfxMuted()
    {
        return isSfxMuted;
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