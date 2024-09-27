using UnityEngine;
using UnityEngine.UI;

public class PauseScreenToggleControls : MonoBehaviour
{
    [Header("Set Toggles")]
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle practiceModeToggle;

    private LoseHealth loseHealth;

    private void Start()
    {
        loseHealth = FindObjectOfType<LoseHealth>();

        if (AudioControl.Instance != null)
        {
            sfxToggle.isOn = PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1;
            musicToggle.isOn = PlayerPrefs.GetInt("Music_Toggle_State", 1) == 1;

            sfxToggle.onValueChanged.AddListener(AudioControl.Instance.OnSfxToggleValueChanged);
            musicToggle.onValueChanged.AddListener(AudioControl.Instance.OnMusicToggleValueChanged);
        }

        if (practiceModeToggle != null)
        {
            practiceModeToggle.isOn = loseHealth.PracticeMode;
            practiceModeToggle.onValueChanged.AddListener(TogglePracticeMode);
        }
    }

    private void OnDestroy()
    {
        if (sfxToggle != null)
            sfxToggle.onValueChanged.RemoveListener(AudioControl.Instance.OnSfxToggleValueChanged);

        if (musicToggle != null)
            musicToggle.onValueChanged.RemoveListener(AudioControl.Instance.OnMusicToggleValueChanged);

        if (practiceModeToggle != null)
            practiceModeToggle.onValueChanged.RemoveListener(TogglePracticeMode);
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