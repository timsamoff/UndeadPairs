using UnityEngine;
using UnityEngine.UI;

public class PauseScreenAudioControl : MonoBehaviour
{
    [Header("Set Toggles")]
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Toggle musicToggle;

    private void Start()
    {
        if (AudioControl.Instance != null)
        {
            sfxToggle.isOn = PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1;
            musicToggle.isOn = PlayerPrefs.GetInt("Music_Toggle_State", 1) == 1;

            sfxToggle.onValueChanged.AddListener(AudioControl.Instance.OnSfxToggleValueChanged);
            musicToggle.onValueChanged.AddListener(AudioControl.Instance.OnMusicToggleValueChanged);
        }
    }

    private void OnDestroy()
    {
        if (sfxToggle != null)
            sfxToggle.onValueChanged.RemoveListener(AudioControl.Instance.OnSfxToggleValueChanged);

        if (musicToggle != null)
            musicToggle.onValueChanged.RemoveListener(AudioControl.Instance.OnMusicToggleValueChanged);
    }
}