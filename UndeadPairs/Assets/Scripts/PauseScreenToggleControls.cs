using UnityEngine;
using UnityEngine.UI;

public class PauseScreenToggleControls : MonoBehaviour
{
    [Header("Set Toggles")]
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle practiceModeToggle;

    private LoseHealth loseHealth;

    private BackgroundMusic backgroundMusic;

    private void Start()
    {
        loseHealth = FindObjectOfType<LoseHealth>();

        if (AudioControl.Instance != null)
        {
            sfxToggle.isOn = PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1;
            musicToggle.isOn = PlayerPrefs.GetInt("Music_Toggle_State", 1) == 1;

            sfxToggle.onValueChanged.AddListener(AudioControl.Instance.OnSfxToggleValueChanged);
            musicToggle.onValueChanged.AddListener(OnMusicToggleValueChanged); // Call local method instead of AudioControl
        }

        if (practiceModeToggle != null)
        {
            practiceModeToggle.isOn = loseHealth.PracticeMode;
            practiceModeToggle.onValueChanged.AddListener(TogglePracticeMode);
        }

        if (backgroundMusic == null)
        {
            backgroundMusic = FindObjectOfType<BackgroundMusic>();
            if (backgroundMusic == null)
            {
                Debug.LogError("BackgroundMusic script not found.");
                return;
            }
        }
    }

    private void OnDestroy()
    {
        if (sfxToggle != null)
            sfxToggle.onValueChanged.RemoveListener(AudioControl.Instance.OnSfxToggleValueChanged);

        if (musicToggle != null)
            musicToggle.onValueChanged.RemoveListener(OnMusicToggleValueChanged);

        if (practiceModeToggle != null)
            practiceModeToggle.onValueChanged.RemoveListener(TogglePracticeMode);
    }

    private void OnMusicToggleValueChanged(bool isOn)
    {
        // Save the new toggle state to PlayerPrefs
        PlayerPrefs.SetInt("Music_Toggle_State", isOn ? 1 : 0);

        if (backgroundMusic != null)
        {
            if (isOn)
            {
                StartCoroutine(backgroundMusic.FadeInMusic());
            }
            else
            {
                StartCoroutine(backgroundMusic.FadeOutMusic());
            }
        }
        else
        {
            Debug.LogError("BackgroundMusic reference not assigned!");
        }
    }

    public void TogglePracticeMode(bool isOn)
    {
        if (loseHealth != null)
        {
            loseHealth.PracticeMode = isOn;
            Debug.Log("Practice mode set to: " + isOn);
        }
        else
        {
            Debug.LogError("LoseHealth component not found!");
        }
    }
}